using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Interfaces;
namespace WikEpubLib
{
    public class Toc : IToc
    {
        public XDocument GetTocFrom(IEnumerable<WikiPageRecord> pageRecords, string bookTitle)
        {
            throw new NotImplementedException();
        }
    }
}
