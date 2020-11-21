using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikEpubLib.Interfaces;
using WikEpubLib.Records;

namespace WikEpubLib.CreateDocs
{
    public class ParseHtml : IParseHtml
    {
        public async Task<(HtmlDocument doc, WikiPageRecord record)> ParseAsync(HtmlDocument htmlDocument, WikiPageRecord wikiPageRecord) =>
            await Task.Run(() =>
            {
                HtmlDocument withContentOnly = ReduceDocument(htmlDocument);
                RemoveLinks(withContentOnly);
                if (wikiPageRecord.SrcMap is null)
                    return (withContentOnly, wikiPageRecord);
                HtmlDocument withAlteredDlLinks = ChangeDownloadLinks(withContentOnly, wikiPageRecord.SrcMap);
                return (withAlteredDlLinks, wikiPageRecord);
                
            });

        private void RemoveLinks(HtmlDocument inputDocument) =>
            inputDocument.DocumentNode.Descendants("a").Distinct().ToList().ForEach(node => {
                HtmlNode newNode = HtmlNode.CreateNode($"<span>{node.InnerText}</span>");
                if (!node.ParentNode.HasClass("reference"))
                    node.ParentNode.ReplaceChild(newNode, node);
            });

        private HtmlDocument ChangeDownloadLinks(HtmlDocument inputDocument, Dictionary<string, string> srcDict)
        {
            inputDocument.DocumentNode.Descendants("img").ToList().ForEach(n =>
            {
                var oldSrcValue = n.GetAttributeValue("src", "null");
                if (srcDict.ContainsKey(oldSrcValue))
                    n.SetAttributeValue("src", srcDict[oldSrcValue]);
            });

            return inputDocument;
        }

        private HtmlDocument ReduceDocument(HtmlDocument inputDocument)
        {
            bool HeadPredicate(HtmlNode node) =>
                node.Name == "meta" & node.Attributes.Any(attribute => attribute.Name == "charset")
                | node.Name == "title";

            bool BodyPredicate(HtmlNode node) =>
                node.Name != "style"
                && !(node.Name == "style" || node.Descendants().Any(d => d.Attributes.Any(a => a.Name == "role")));

            var bodyString = GetHtmlString(inputDocument, "//*[@id='mw-content-text']/div[1]", BodyPredicate, "body");
            var headString = GetHtmlString(inputDocument, "//html/head", HeadPredicate, "head");
            var htmlString =
                string.Join(
                    "",
                    new List<string> { headString, bodyString }
                        .Prepend("<!DOCTYPE html><html>")
                        .Append("</html>")
                    );
            HtmlDocument newDoc = new HtmlDocument();
            newDoc.LoadHtml(htmlString);
            return newDoc;
        }

        private string GetHtmlString(HtmlDocument inputDocument, string xPathToParent, Predicate<HtmlNode> nodePredicate,
            string encapsulateWithNode)
        {
            var nodeStrings = new List<string>();
            var currentNode = inputDocument.DocumentNode.SelectSingleNode(xPathToParent).FirstChild;
            while (currentNode != null)
            {
                if (nodePredicate(currentNode))
                    nodeStrings.Add(currentNode.OuterHtml);
                currentNode = currentNode.NextSibling;
            }
            return string.Join("", nodeStrings.Prepend($"<{encapsulateWithNode}>").Append($"</{encapsulateWithNode}>"));
        }
    }
}