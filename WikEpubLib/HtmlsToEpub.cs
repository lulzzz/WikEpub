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
            Console.WriteLine("Getting docs");
            Task<HtmlDocument[]> initialDocs = _htmlInput.GetHtmlDocuments(withUrls, new HtmlWeb());

            var directories = GetDirectoryDict(rootDirectory, folderID);

            Console.WriteLine("Creating directores");
            Task createDirectories = _epubOutput.CreateDirectories(rootDirectory, folderID);

            Console.WriteLine("Creating records");
            IEnumerable<(HtmlDocument doc, Task<WikiPageRecord> record)> htmlRecordTuple = 
                (await initialDocs).Select(doc => (doc, _getRecords.From(doc, "image_repo")));

            // Download images here
            
            Console.WriteLine("creating xml docs"); 
            var pageRecords = htmlRecordTuple.Select(t => t.record.Result);
            Task<IEnumerable<(XmlType type, XDocument doc)>> xmlDocs =  _getXmlDocs.FromAsync(pageRecords, bookTitle);
            

            Console.WriteLine("parsing html");
            IEnumerable<(Task<HtmlDocument> doc, WikiPageRecord record)> parsedDocuments = 
                htmlRecordTuple.Select(t  => (_parseHtml.ParseAsync(t.doc, t.record.Result), t.record.Result));
            

            await createDirectories;
            Console.WriteLine("Saving files to directory");
            await _epubOutput.SaveToAsync(directories, xmlDocs.Result, parsedDocuments.Select(t => (t.doc.Result, t.record)));

            Console.WriteLine("Saved");
            // save files here
            
            throw new NotImplementedException();
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
