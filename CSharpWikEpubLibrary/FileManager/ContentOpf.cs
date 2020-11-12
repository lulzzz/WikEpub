using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml;

namespace CSharpWikEpubLibrary.FileManager
{
    class ContentOpf : IContentOpf
    {
        public void Create(Dictionary<HtmlDocument, string> htmlInfo, string inDirectory, string bookTitle)
        {
            throw new NotImplementedException();
        }


        IEnumerable<HtmlNode> DocumentNodes(HtmlDocument document) => document.DocumentNode.Descendants();


        private List<string> GetImagePaths(IEnumerable<HtmlNode> nodes) =>
            nodes
                .Where(node => node.Name == "img")
                .Select(node => node.GetAttributeValue("src", "no_value")).Distinct()
                .Select(path => string.Join(@"\", path.Split(@"\")[^2..]))
                .ToList();
    }
}
