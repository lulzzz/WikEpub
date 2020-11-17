using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WikEpubLib.Interfaces;
using System.IO;

namespace WikEpubLib
{
    class HtmlsToEpub : IHtmlsToEpub
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
            HtmlDocument[] initialDocs = await GetHtmlDocuments(withUrls, new HtmlWeb());


            // create directorys async
            Guid guid = Guid.NewGuid();
            Task createDirectories = CreateDirectories(toRootDirectory, guid);
            var directories = GetDirectoryDict(toRootDirectory, guid);

            IEnumerable<(HtmlDocument html, WikiPageRecord pageRecord)> htmlRecordTuple = 
                initialDocs.AsParallel().Select(doc => (doc, _getRecords.From(doc, "image_directory")));


            // wait for directories
            
            // create xml files

            // do IO bound work here
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
            {Directories.BOOKDIR,  @$"{rootDir}\{id}" }
        };
    }

    internal enum Directories
    {
        ROOT,
        OEBPS,
        METAINF,
        BOOKDIR
    }
}
