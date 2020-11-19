using System.Collections.Generic;
using System.Xml.Linq;
using WikEpubLib.Records;

namespace WikEpubLib.Interfaces
{
    public interface IGetContentXml
    {
        XDocument From(IEnumerable<WikiPageRecord> wikiPageRecords, string bookTitle);
    }
}