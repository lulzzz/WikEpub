using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
        private readonly IContainer _container;

        public ConvertEpub(IParseHtmlDoc getEpubHtml, IProcessImages processImages, IContentOpf contentOpf, IToc toc, IContainer container)
        {
            _getEpubHtml = getEpubHtml;
            _processImages = processImages;
            _contentOpf = contentOpf;
            _toc = toc;
            _container = container;
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

            var initialEpubDocTasks = initialHtmlDocs.Select(doc => _getEpubHtml.TransformAsync(doc));
            var initialEpubDocs = Task.WhenAll(initialEpubDocTasks);

            // Get unique folder names for each html files
            int imgDirNum = 1;
            string GetImgDir() => @$"img_dir{imgDirNum++}\";

            await initialEpubDocs;
            var processImageDownloadTasks =
                initialEpubDocs
                    .Result
                    .Select(doc => 
                        _processImages.ProcessImageDownloadsAsync(doc,  @$"{rootDirectory}{bookTitle}\OEBPS\" + GetImgDir()));
            var finalHtmlDocs = Task.WhenAll(processImageDownloadTasks);

            var htmlIdDict = await GetHtmlIdDict(finalHtmlDocs.Result);

            await SaveFiles(rootDirectory, bookTitle, htmlIdDict, finalHtmlDocs);

            await CompressFiles(@$"{rootDirectory}{bookTitle}", bookTitle);

            Directory.Delete(@$"{rootDirectory}{bookTitle}", true);
        }

        private async Task CompressFiles(string bookDirectory, string bookTitle) =>  
            await Task.Run(() => 
                ZipFile.CreateFromDirectory(
                    bookDirectory, 
                    @$"{string.Join(@"\",bookDirectory.Split('\\')[..^1])}\{bookTitle}.epub")
                );

        private async Task SaveFiles(string rootDirectory, string bookTitle, Dictionary<HtmlDocument, string> htmlIdDict,
            Task<HtmlDocument[]> htmlDocumentsAfterImageProcessing)
        {
            string oebpsDir = @$"{rootDirectory}{bookTitle}\OEBPS\";
            string bookDir = $@"{rootDirectory}{bookTitle}\";
            await Task.WhenAll(new List<Task>
            {
                _container.CreateAsync(bookDir),
                _contentOpf.CreateAsync(htmlIdDict, oebpsDir, bookTitle),
                _toc.CreateAsync(htmlIdDict, oebpsDir, bookTitle),
                SaveHtmlDocsAsync(oebpsDir, htmlDocumentsAfterImageProcessing.Result, htmlIdDict),
                CreatMimeTypeAsync(bookDir)
            });
        }

        private async Task SaveHtmlDocsAsync(string oebpsDirectory, HtmlDocument[] documents, Dictionary<HtmlDocument, string> htmlId) =>  
            await documents.ForEachAsync(async d =>
            {
                await using var fileStream = File.Create(@$"{oebpsDirectory}\{htmlId[d]}.html");
                await Task.Run(() => d.Save(fileStream));
            });

        private async Task CreatMimeTypeAsync(string rootDirectory) =>
            await File.WriteAllTextAsync($@"{rootDirectory}mimetype", "application/epub+zip");


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
