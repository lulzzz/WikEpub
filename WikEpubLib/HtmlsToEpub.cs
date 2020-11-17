using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WikEpubLib.Interfaces;
using System.IO;
using System.Xml.Linq;

namespace WikEpubLib
{
    class HtmlsToEpub : IHtmlsToEpub
    {
        IParseHtml _parseHtml;
        IGetWikiPageRecords _getRecords;
        readonly IGetXmlDocs _getXmlDocs;

        public HtmlsToEpub(IParseHtml parseHtml, IGetWikiPageRecords getRecords, IGetXmlDocs getXmlDocs)
        {
            _parseHtml = parseHtml;
            _getRecords = getRecords;
            _getXmlDocs = getXmlDocs;
        }

        public async Task Transform(IEnumerable<string> withUrls, string rootDirectory, string bookTitle)
        {
            Task<HtmlDocument[]> initialDocs = GetHtmlDocuments(withUrls, new HtmlWeb());

            Guid guid = Guid.NewGuid();
            Task createDirectories = CreateDirectories(rootDirectory, guid);
            var directories = GetDirectoryDict(rootDirectory, guid);

            IEnumerable<(HtmlDocument html, WikiPageRecord pageRecord)> htmlRecordTuple = 
                (await initialDocs).AsParallel().Select(doc => (doc, _getRecords.From(doc, "image_repo")));

            var pageRecords = htmlRecordTuple.Select(t => t.pageRecord);
            Task<Dictionary<XmlType, XDocument>> xmlDocs =  _getXmlDocs.From(pageRecords, bookTitle);   

            IEnumerable<HtmlDocument> parsedDocuments = 
                htmlRecordTuple.AsParallel().Select(t => _parseHtml.Parse(t.html, t.pageRecord));
            
            // save files here
            
            throw new NotImplementedException();
        }

        private async Task<HtmlDocument[]> GetHtmlDocuments(IEnumerable<string> urls, HtmlWeb htmlWeb) =>
            await Task.WhenAll(urls.Select(url => htmlWeb.LoadFromWebAsync(url)));

        private async Task CreateDirectories(string rootDirectory, Guid id) =>
            await Task.Run(() =>
            {
                Directory.CreateDirectory(@$"{rootDirectory}\{id}\OEBPS");
                Directory.CreateDirectory(@$"{rootDirectory}\{id}\META-INF");
            });

        private Dictionary<Directories, string> GetDirectoryDict(string rootDir, Guid id) => new Dictionary<Directories, string> {
            {Directories.ROOT, rootDir},
            {Directories.OEBPS, @$"{rootDir}\{id}\OEBPS" },
            {Directories.METAINF, @$"{rootDir}\{id}\META-INF" },
            {Directories.BOOKDIR,  @$"{rootDir}\{id}" },
            {Directories.IMAGES, @$"{rootDir}\{id}\OEBPS\image_repo" }
        };
    }

    internal enum Directories
    {
        ROOT,
        OEBPS,
        METAINF,
        BOOKDIR,
        IMAGES
    }
}
