using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Enums;
using WikEpubLib.Records;

namespace WikEpubLib.Interfaces
{
    public interface IGetContentXml
    {
        Task<(XmlType, XDocument)> From(IEnumerable<WikiPageRecord> wikiPageRecords, string bookTitle);
    }
}