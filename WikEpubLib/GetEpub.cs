using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Enums;
using WikEpubLib.Interfaces;
using WikEpubLib.Records;

namespace WikEpubLib
{
    public class GetEpub : IHtmlsToEpub
    {
        private readonly IHtmlInput _htmlInput;
        private readonly IParseHtml _parseHtml;
        private readonly IGetWikiPageRecords _getRecords;
        private readonly IGetXmlDocs _getXmlDocs;
        private readonly IEpubOutput _epubOutput;

        public GetEpub(IParseHtml parseHtml, IGetWikiPageRecords getRecords,
            IGetXmlDocs getXmlDocs, IHtmlInput htmlInput, IEpubOutput epubOutput)
        {
            _parseHtml = parseHtml;
            _getRecords = getRecords;
            _getXmlDocs = getXmlDocs;
            _htmlInput = htmlInput;
            _epubOutput = epubOutput;
        }

        public async Task FromAsync(IEnumerable<string> urls, string rootDirectory, string bookTitle, Guid folderID)
        {
            Task<HtmlDocument[]> initialDocs = _htmlInput.GetHtmlDocumentsFromAsync(urls, new HtmlWeb());

            var directories = GetDirectoryDict(rootDirectory, folderID);
            Task createDirectories = _epubOutput.CreateDirectoriesAsync(directories);

            List<(HtmlDocument doc, WikiPageRecord record)> htmlRecordTuple =
               (await initialDocs).Select(doc => (doc, _getRecords.From(doc, "image_repo"))).ToList();

            var pageRecords = htmlRecordTuple.Select(t => t.record);
            Task downloadImagesTask = Task.WhenAll(pageRecords.SelectMany(record => _epubOutput.DownLoadImagesAsync(record, directories)));
            //convert to IEnumerable<Task> so can be run concurrently
            Task<IEnumerable<(XmlType type, XDocument doc)>> xmlDocs = _getXmlDocs.FromAsync(pageRecords, bookTitle);

            IEnumerable<(Task<HtmlDocument> doc, WikiPageRecord record)> parsedDocuments =
                htmlRecordTuple.Select(t => (_parseHtml.ParseAsync(t.doc, t.record), t.record));

            await createDirectories;

            Task createMime = _epubOutput.CreateMimeFile(directories);

            // this should be seperated into differnet calls so that they don't have to wait (one for xml, one for html)
            await _epubOutput.SaveDocumentsAsync(directories, xmlDocs.Result, parsedDocuments.Select(t => (t.doc.Result, t.record)));
            await downloadImagesTask;
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