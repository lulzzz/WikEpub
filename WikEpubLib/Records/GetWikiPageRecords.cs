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
            IEnumerable<HtmlNode> contentNodes = allNodes.First(n => n.Name == "body").Descendants().Distinct();
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
        // need to handle svg files too once they are being converted
        private string GetImageId(string originalSrc) => $"image_{_imageId++}.{GetExtension(originalSrc)}";
        private string GetExtension(string originalSrc) => originalSrc switch
        {
            // this can be more efficient instead of checking svg every time
            // maybe create hashset of expected formats, then check
            string when originalSrc.Contains("svg") => "svg.png",
            _ => originalSrc.Split('.')[^1] + ".png"    
        };
        //private string GetImageId() => $"image_{_imageId++}.png";
        private Dictionary<string, string> GetSrcMapFrom(IEnumerable<HtmlNode> imageNodes, string imageDirectory) =>
            imageNodes
            .Select(n => n.GetAttributeValue("src", "null"))
            .Distinct().ToDictionary(src => src, src => @$"{imageDirectory}\{GetImageId(src)}");

        private List<(string id, string sectionName)> GetSectionHeadingsFrom(IEnumerable<HtmlNode> nodes) =>
            nodes
            .Where(n => n.Name == "h2")
            .Select(n => ($"#{n.GetAttributeValue("id", "null")}", n.InnerText)).ToList();
    }
}