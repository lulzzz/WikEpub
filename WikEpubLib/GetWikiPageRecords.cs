using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;

namespace WikEpubLib
{
    internal class GetWikiPageRecords
    {
        public GetWikiPageRecords(HtmlDocument html, string rootDirectory)
        {
        }

        private string GetId(IEnumerable<HtmlNode> nodes) =>
            nodes.First(n => n.Name == "title").InnerHtml.Split('-').First().Trim().Replace(' ', '_');
    }
}