using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikEpubLib.Interfaces;
using WikEpubLib.Records;

namespace WikEpubLib.CreateDocs
{
    public class HtmlParser : IParseHtml
    {

        public async Task<(HtmlDocument doc, WikiPageRecord record)> ParseAsync(HtmlDocument htmlDocument, WikiPageRecord wikiPageRecord)
        {
            return await Task.Run(() =>
            {
                return (CreateHtml(htmlDocument, wikiPageRecord), wikiPageRecord);
            });
        }

        public HtmlDocument CreateHtml(HtmlDocument inputDocument, WikiPageRecord wikiPageRecord)
        {
            HtmlDocument newDocument = new HtmlDocument();
            var initNode =
                HtmlNode.CreateNode($"<html><head><meta charset=\"utf-8\"><title>{wikiPageRecord.Id.Replace('_', ' ')}</title></head><body></body></html>");
            newDocument.DocumentNode.AppendChild(initNode);
            var bodyNode = newDocument.DocumentNode.SelectSingleNode("/html/body");
            
            bool nodePredicate(HtmlNode node) => node.Name != "style"
                && !(node.Name == "style" || node.Descendants().Any(d => d.Attributes.Any(a => a.Name == "role")));

            var childNodes = inputDocument
                .DocumentNode
                .SelectSingleNode("//*[@id='mw-content-text']/div[1]")
                .ChildNodes;

            
            foreach (var node in childNodes)
            {
                if (nodePredicate(node))
                {
                    ChangeDownloadLinks(node, wikiPageRecord.SrcMap);
                    bodyNode.AppendChild(node);

                }
                                }
            return newDocument;
        }
        
        private void ChangeDownloadLinks(HtmlNode node, Dictionary<string, string> srcMap)
        {
            // and self returns the parent node regardless of match, juse use descendants and check if the parent node is img
            var imgNodes = node.DescendantsAndSelf("img");
            if (!imgNodes.Any()) return;
            foreach (var imgNode in imgNodes)
            {
                var oldSrcValue = node.GetAttributeValue("src", "null");
                if (srcMap.ContainsKey(oldSrcValue))
                    imgNode.SetAttributeValue("src", srcMap[oldSrcValue]);
            }
        }
    }
}
