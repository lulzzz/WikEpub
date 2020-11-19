using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Enums;
using WikEpubLib.Records;

namespace WikEpubLib
{
    public interface IEpubOutput
    {
        Task CreateDirectoriesAsync(Dictionary<Directories, string> directories);

        Task SaveDocumentAsync(Dictionary<Directories, string> directories, IEnumerable<(XmlType type, XDocument doc)> xmlDocs, IEnumerable<(HtmlDocument doc, WikiPageRecord record)> htmlDocuments);

        IEnumerable<Task> DownLoadImagesAsync(WikiPageRecord pageRecord, Dictionary<Directories, string> directories);

        Task ZipFiles(Dictionary<Directories, string> directories, Guid guid);

        Task CreateMimeFile(Dictionary<Directories, string> directories);
    }
}