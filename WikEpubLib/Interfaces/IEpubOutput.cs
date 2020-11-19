using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WikEpubLib
{
    public interface IEpubOutput
    {

        Task CreateDirectories(Dictionary<Directories, string> directories);

        Task SaveToAsync(Dictionary<Directories, string> directories, IEnumerable<(XmlType type, XDocument doc)> xmlDocs, IEnumerable<(HtmlDocument doc, WikiPageRecord record)> htmlDocuments);

        Task DownLoadImagesAsync(WikiPageRecord pageRecord, Dictionary<Directories, string> directories);

        Task ZipFiles(Dictionary<Directories, string> directories, Guid guid);

        Task CreateMimeFile(Dictionary<Directories, string> directories);
    }
}

