using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpWikEpubLibrary.FileManager;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    class ConvertEpub : IConvertEpub
    {
        private IParseHtmlDoc _getEpubHtml;
        private IProcessImages _processImages;
        private IContentOpf _contentOpf;
        private IToc _toc;
        private HttpClient _httpClient;

        public ConvertEpub(IParseHtmlDoc getEpubHtml, IProcessImages processImages, IContentOpf contentOpf, IToc toc, HttpClient httpClient)
        {
            _getEpubHtml = getEpubHtml;
            _processImages = processImages;
            _contentOpf = contentOpf;
            _toc = toc;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Fetches html documents from list of url arguments and converts them to a single epub file at the directory specified.
        /// </summary>
        /// <param name="urls">urls to fetch wikipedia html documents from</param>
        /// <param name="rootDirectory">root directory destination of epub document</param>
        /// <returns></returns>
        public async Task ConvertAsync(IEnumerable<string> urls, string rootDirectory)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            var getInitialHtmlTasks = urls.Select(url => GetHtmlDocument(url, htmlWeb));
            var initialHtmlDocs = Task.WhenAll(getInitialHtmlTasks);

            

            var initialEpubDocTasks = initialHtmlDocs.Result.Select( d => _getEpubHtml.TransformAsync(d));
            var initialEpubDocs = Task.WhenAll(initialEpubDocTasks);
            var htmlIdDict = GetHtmlIdDict(initialHtmlDocs.Result);


            var processImageDownloadTasks =
                initialEpubDocs.Result.Select(d => _processImages.ProcessDownloadLinks(d, htmlIdDict.Result[d]));

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
                .Trim();



    }

}
