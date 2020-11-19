using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using WikEpubLib.Interfaces;

namespace WikEpubLib.Records
{
    public class GetWikiPageRecords : IGetWikiPageRecords
    {
        public WikiPageRecord From(HtmlDocument html, string imageDirectory)
        {
            IEnumerable<HtmlNode> allNodes = html.DocumentNode.Descendants();
            IEnumerable<HtmlNode> contentNodes = allNodes.First(n => n.GetAttributeValue("id", "null") == "mw-content-text").FirstChild.Descendants();
            IEnumerable<HtmlNode> imgNodes = GetImageNodesFrom(contentNodes);
            return new WikiPageRecord
            {
                Id = GetIdFrom(allNodes),
                SrcMap = imgNodes.Any() ? GetSrcMapFrom(imgNodes, imageDirectory) : null,
                SectionHeadings = GetSectionHeadingsFrom(contentNodes)
            };
        }

        private string GetIdFrom(IEnumerable<HtmlNode> nodes) =>
            nodes
            .First(n => n.Name == "title")
            .InnerHtml.Split('-').First()
            .Trim().Replace(' ', '_').Replace(")", "").Replace("(", "");

        private IEnumerable<HtmlNode> GetImageNodesFrom(IEnumerable<HtmlNode> nodes) => nodes.Where(n => n.Name == "img");

        private int _imageId = 1;

        private string GetImageId(string originalSrc) => $"image_{_imageId++}.{originalSrc.Split('.')[^1]}";

        private Dictionary<string, string> GetSrcMapFrom(IEnumerable<HtmlNode> imageNodes, string imageDirectory) =>
            imageNodes.AsParallel()
            .Select(n => n.GetAttributeValue("src", "null"))
            .Distinct().ToDictionary(s => s, s => @$"{imageDirectory}\{GetImageId(s)}");

        private List<(string id, string sectionName)> GetSectionHeadingsFrom(IEnumerable<HtmlNode> nodes) =>
            nodes.AsParallel().AsOrdered()
            .Where(n => n.Name == "h2")
            .Select(n => n.FirstChild)
            .Select(n => ($"#{n.GetAttributeValue("id", "null")}", n.InnerHtml)).ToList();
    }
}