using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Interfaces;

namespace WikEpubLib
{
    public class HtmlsToEpub : IHtmlsToEpub
    {
        private readonly IHtmlInput _htmlInput;
        private readonly IParseHtml _parseHtml;
        private readonly IGetWikiPageRecords _getRecords;
        private readonly IGetXmlDocs _getXmlDocs;
        private readonly IEpubOutput _epubOutput;

        public HtmlsToEpub(IParseHtml parseHtml, IGetWikiPageRecords getRecords,
            IGetXmlDocs getXmlDocs, IHtmlInput htmlInput, IEpubOutput epubOutput)
        {
            _parseHtml = parseHtml;
            _getRecords = getRecords;
            _getXmlDocs = getXmlDocs;
            _htmlInput = htmlInput;
            _epubOutput = epubOutput;
        }

        public async Task GetEpub(IEnumerable<string> fromUrls, string rootDirectory, string bookTitle, Guid folderID)
        {
            Task<HtmlDocument[]> initialDocs = _htmlInput.GetHtmlDocuments(fromUrls, new HtmlWeb());

            var directories = GetDirectoryDict(rootDirectory, folderID);
            Task createDirectories = _epubOutput.CreateDirectories(directories);

            List<(HtmlDocument doc, WikiPageRecord record)> htmlRecordTuple =
               (await initialDocs).Select(doc => (doc, _getRecords.From(doc, "image_repo"))).ToList();

            var pageRecords = htmlRecordTuple.Select(t => t.record);
            Task downloadImages = pageRecords.ForEachAsync(record => _epubOutput.DownLoadImagesAsync(record, directories));
            Task<IEnumerable<(XmlType type, XDocument doc)>> xmlDocs = _getXmlDocs.FromAsync(pageRecords, bookTitle);

            IEnumerable<(Task<HtmlDocument> doc, WikiPageRecord record)> parsedDocuments =
                htmlRecordTuple.Select(t => (_parseHtml.ParseAsync(t.doc, t.record), t.record));

            await createDirectories;

            Task createMime = _epubOutput.CreateMimeFile(directories);
            await _epubOutput.SaveToAsync(directories, xmlDocs.Result, parsedDocuments.Select(t => (t.doc.Result, t.record)));
            await downloadImages;
            await createMime;

            await _epubOutput.ZipFiles(directories, folderID);

        }

        
        private Dictionary<Directories, string> GetDirectoryDict(string rootDir, Guid folderId) => new Dictionary<Directories, string> {
            {Directories.ROOT, rootDir},
            {Directories.OEBPS, @$"{rootDir}\{folderId}\OEBPS" },
            {Directories.METAINF, @$"{rootDir}\{folderId}\META-INF" },
            {Directories.BOOKDIR,  @$"{rootDir}\{folderId}" },
            {Directories.IMAGES, @$"{rootDir}\{folderId}\OEBPS\image_repo" }
        };
    }

}