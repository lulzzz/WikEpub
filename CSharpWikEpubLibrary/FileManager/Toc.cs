using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.FileManager
{
    public class Toc: IToc
    {
        public void Create(Dictionary<HtmlDocument, string> htmlIds, string toDirectory)
        {
            throw new NotImplementedException();
        }

        public XDocument GetTocDocument(Dictionary<HtmlDocument, string> htmlIds)
        {

            throw new NotImplementedException();
        }
    }
}
