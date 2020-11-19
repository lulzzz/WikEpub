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
    public class HtmlsToEpub : IHtmlsToEpub
    {
        readonly IHtmlInput _htmlInput;
        readonly IParseHtml _parseHtml;
        readonly IGetWikiPageRecords _getRecords;
        readonly IGetXmlDocs _getXmlDocs;
        readonly IEpubOutput _epubOutput;

        public HtmlsToEpub(IParseHtml parseHtml, IGetWikiPageRecords getRecords,
            IGetXmlDocs getXmlDocs, IHtmlInput htmlInput, IEpubOutput epubOutput)
        {
            _parseHtml = parseHtml;
            _getRecords = getRecords;
            _getXmlDocs = getXmlDocs;
            _htmlInput = htmlInput;
            _epubOutput = epubOutput;
        }

        public async Task Transform(IEnumerable<string> withUrls, string rootDirectory, string bookTitle, Guid folderID)
        {
            Task<HtmlDocument[]> initialDocs = _htmlInput.GetHtmlDocuments(withUrls, new HtmlWeb());

            var directories = GetDirectoryDict(rootDirectory, folderID);
            Task createDirectories = _epubOutput.CreateDirectories(rootDirectory, folderID);

            List<(HtmlDocument doc, WikiPageRecord record)> htmlRecordTuple = 
               (await initialDocs).Select(doc => ( doc, _getRecords.From(doc, "image_repo"))).ToList();

            var pageRecords = htmlRecordTuple.Select( t => t.record);
            Task downLoadImages = pageRecords.AsParallel().WithDegreeOfParallelism(10).ForEachAsync(record => _epubOutput.DownLoadImagesAsync(record, directories));
            Task<IEnumerable<(XmlType type, XDocument doc)>> xmlDocs = _getXmlDocs.FromAsync(pageRecords, bookTitle);


            IEnumerable<(Task<HtmlDocument> doc, WikiPageRecord record)> parsedDocuments =
                htmlRecordTuple.Select(t => (_parseHtml.ParseAsync(t.doc, t.record), t.record));


            await createDirectories;
            await _epubOutput.SaveToAsync(directories, xmlDocs.Result, parsedDocuments.Select(t => (t.doc.Result, t.record)));
            await downLoadImages;
          
            // save files here

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
