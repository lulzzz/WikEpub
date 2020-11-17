using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Interfaces;
namespace WikEpubLib
{
    public class Content : IContent
    {
        public XDocument GetContentFrom(IEnumerable<WikiPageRecord> wikiPageRecords, string bookTitle)
        {
            throw new NotImplementedException();
        }
    }
}
