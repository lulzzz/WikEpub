using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WikEpubLib
{
    public interface IGetXmlDocs
    {
        Task<Dictionary<XmlType, XDocument>> From(IEnumerable<WikiPageRecord> pageRecords, string bookTitle);
    }
}
