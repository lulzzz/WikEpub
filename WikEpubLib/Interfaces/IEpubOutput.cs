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
        Task CreateDirectories(string rootDirectory, Guid folderID);

        Task SaveToAsync(string rootDirectory, Guid folderId, IEnumerable<(XmlType type, XDocument doc)> xmlDocs, IEnumerable<(HtmlDocument doc, WikiPageRecord record)> htmlDocuments);

    }
}

