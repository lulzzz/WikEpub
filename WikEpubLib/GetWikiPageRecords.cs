using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace WikEpubLib
{
    public class GetWikiPageRecords
    {
        public WikiPageRecord GetRecordsFrom(HtmlDocument html, string imageDirectory)
        {
            IEnumerable<HtmlNode> nodes = html.DocumentNode.Descendants();
            IEnumerable<HtmlNode> imgNodes = GetImageNodes(nodes);
            return new WikiPageRecord
            {
                Id = GetId(nodes),
                SrcMap = imgNodes.Any() ? GetSrcMap(imgNodes, imageDirectory) : null,
                SectionHeadings = GetSectionHeadings(nodes)
            };
        }

        private string GetId(IEnumerable<HtmlNode> nodes) =>
            nodes.First(n => n.Name == "title").InnerHtml.Split('-').First().Trim().Replace(' ', '_').Replace(")", "").Replace("(", "");

        private IEnumerable<HtmlNode> GetImageNodes(IEnumerable<HtmlNode> nodes) => nodes.Where(n => n.Name == "img");

        private int _imageId = 1;
        private string GetImageId(string originalSrc) => $"image_{_imageId++}.{originalSrc.Split('.')[^1]}";
        private Dictionary<string, string> GetSrcMap(IEnumerable<HtmlNode> imageNodes, string imageDirectory) =>
            imageNodes.Select(n => n.GetAttributeValue("src", "null")).Distinct().ToDictionary(s => s, s => @$"{imageDirectory}\{GetImageId(s)}");

        private IEnumerable<(string id, string sectionName)> GetSectionHeadings(IEnumerable<HtmlNode> nodes) =>
            nodes.Where(n => n.Name == "h2")
            .Select(n => n.FirstChild)
            .Select(n => ($"#{n.GetAttributeValue("id", "null")}", n.InnerHtml));
    }
}