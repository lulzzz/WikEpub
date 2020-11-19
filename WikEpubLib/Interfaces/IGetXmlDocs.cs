using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Enums;
using WikEpubLib.Records;

namespace WikEpubLib
{
    public interface IGetXmlDocs
    {
        Task<IEnumerable<(XmlType type, XDocument doc)>> FromAsync(IEnumerable<WikiPageRecord> pageRecords, string bookTitle);
    }
}
