using System.Collections.Generic;
using System.Xml.Linq;
using WikEpubLib.Records;

namespace WikEpubLib.Interfaces
{
    public interface IGetTocXml
    {
        XDocument From(IEnumerable<WikiPageRecord> pageRecords, string bookTitle);
    }
}