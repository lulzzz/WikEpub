using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpWikEpubLibrary.FileManager;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public class ConvertEpub : IConvertEpub
    {
        private readonly IParseHtmlDoc _getEpubHtml;
        private readonly IProcessImages _processImages;
        private readonly IContentOpf _contentOpf;
        private readonly IToc _toc;

        public ConvertEpub(IParseHtmlDoc getEpubHtml, IProcessImages processImages, IContentOpf contentOpf, IToc toc)
        {
            _getEpubHtml = getEpubHtml;
            _processImages = processImages;
            _contentOpf = contentOpf;
            _toc = toc;
        }

        /// <summary>
        /// Fetches html documents from list of url arguments and converts them to a single epub file at the directory specified.
        /// </summary>
        /// <param name="urls">urls to fetch wikipedia html documents from</param>
        /// <param name="rootDirectory">root directory destination of epub document</param>
        /// <param name="bookTitle"></param>
        /// <returns></returns>
        public async Task ConvertAsync(IEnumerable<string> urls, string rootDirectory, string bookTitle)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            var getInitialHtmlTasks = urls.Select(url => GetHtmlDocument(url, htmlWeb));
            var initialHtmlDocs = await Task.WhenAll(getInitialHtmlTasks);

            var initialEpubDocTasks = initialHtmlDocs.Select(d => _getEpubHtml.TransformAsync(d));
            var initialEpubDocs = Task.WhenAll(initialEpubDocTasks);

            int imgDirNum = 1;
            string GetImgDir() => @$"img_dir{imgDirNum++}\";

            await initialEpubDocs;
            var processImageDownloadTasks =
                initialEpubDocs.Result.Select(d => _processImages.ProcessDownloadLinks(d,  @$"{rootDirectory}{bookTitle}\OEBPS\" + GetImgDir()));
            var htmlDocumentsAfterImageProcessing = Task.WhenAll(processImageDownloadTasks);

            var htmlIdDict = await GetHtmlIdDict(htmlDocumentsAfterImageProcessing.Result);

            string oebpsDir = @$"{rootDirectory}{bookTitle}\OEBPS\";
            var saveDocumentTasks = SaveHtmlDocs(oebpsDir, htmlDocumentsAfterImageProcessing.Result, htmlIdDict);
            var getContentTask = _contentOpf.Create(htmlIdDict,oebpsDir, bookTitle);
            var getTocTask = _toc.Create(htmlIdDict,oebpsDir, bookTitle);

            await Task.WhenAll(new List<Task>{getContentTask, getTocTask, saveDocumentTasks, CreatMimeType( @$"{rootDirectory}{bookTitle}\")});

        }

        private async Task SaveHtmlDocs(string oebpsDirectory, HtmlDocument[] documents, Dictionary<HtmlDocument, string> htmlId)
        {
            await documents.ForEachAsync(async d =>
            {
                await using var fileStream = File.Create(@$"{oebpsDirectory}\{htmlId[d]}.html");
                await Task.Run(() => d.Save(fileStream));
            });
        }

        private async Task CreatMimeType(string rootDirectory)
        { 
            await File.WriteAllTextAsync($@"{rootDirectory}mimetype", "application/epub+zip");
        }


        private Task<HtmlDocument> GetHtmlDocument(string url, HtmlWeb getHtml) =>
            getHtml.LoadFromWebAsync(url);


        private async Task<Dictionary<HtmlDocument,string>> GetHtmlIdDict(IEnumerable<HtmlDocument> htmlDocuments) =>
            await Task.Run(() => htmlDocuments.ToDictionary( keySelector: document =>  document, GetWikiTitle));


        string GetWikiTitle(HtmlDocument inputDocument) =>
            inputDocument
                .DocumentNode
                .Descendants()
                .First(node => node.Name == "title")
                .InnerHtml
                .Split('-')
                .First()
                .Trim()
                .Replace(' ', '_');



    }

}
