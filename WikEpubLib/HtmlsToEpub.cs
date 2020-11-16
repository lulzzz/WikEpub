using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WikEpubLib
{
    class HtmlsToEpub
    {
        IParseHtml _parseHtml;
        IGetWikiPageRecords _getRecords;

        public HtmlsToEpub(IParseHtml parseHtml, IGetWikiPageRecords getRecords)
        {
            _parseHtml = parseHtml;
            _getRecords = getRecords;
        }

        public async Task Transform(IEnumerable<string> withUrls, string toRootDirectory, string asBookTitle)
        {
            HtmlDocument[] initialDocs = await GetHtmlDocuments(withUrls);

            int imgDirNum = 1;
            string GetImageDir() => $"image_directory_{imgDirNum++}";
            IEnumerable<(HtmlDocument html, WikiPageRecord pageRecord)> docRecordDict = 
                initialDocs.AsParallel().Select(doc => (doc, _getRecords.From(doc, GetImageDir())));

            // do IO bound work here
            IEnumerable<HtmlDocument> parsedDocuments = docRecordDict.AsParallel().Select(t => _parseHtml.Parse(t.html, t.pageRecord));
                
            throw new NotImplementedException();
        }

        private async Task<HtmlDocument[]> GetHtmlDocuments(IEnumerable<string> urls)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            return await Task.WhenAll(urls.Select(url => htmlWeb.LoadFromWebAsync(url)));
        }

        
        
    }
}
