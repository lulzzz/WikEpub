using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikEpubLib.Interfaces;

namespace WikEpubLib
{
    public class ParseHtml: IParseHtml
    {
        public async Task<HtmlDocument> ParseAsync(HtmlDocument htmlDocument, WikiPageRecord wikiPageRecord) =>
            await Task.Run(()=> { 
                var reducedDocument = ReduceDocument(htmlDocument);
                if (wikiPageRecord.SrcMap is null)
                    return reducedDocument;
                var html = ChangeDownloadLinks(reducedDocument, wikiPageRecord.SrcMap);
                return html;
            });
           

        private HtmlDocument ChangeDownloadLinks(HtmlDocument inputDocument, Dictionary<string, string> srcDict)
        {
            inputDocument.DocumentNode.Descendants().AsParallel().Where(n => n.Name == "img").ToList().ForEach(n =>
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
                (node.Name == "meta" & node.Attributes.Any(attribute => attribute.Name == "charset")) 
                | node.Name == "title";

            bool BodyPredicate(HtmlNode node) =>
                node.Name != "style"
                & (node.Attributes.All(attribute => attribute.Name != "role")
                | node.Attributes.Any(attribute => attribute.Value == "toc"));

            var bodyString = GetHtmlString(inputDocument, "//*[@id='mw-content-text']/div[1]", BodyPredicate, "body");
            var headString = GetHtmlString(inputDocument, "//html/head", HeadPredicate, "head");
            var htmlString = 
                string.Join(
                    "", 
                    new List<string>{headString, bodyString}
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
