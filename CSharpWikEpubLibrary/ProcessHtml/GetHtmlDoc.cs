using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public class GetHtmlDoc : IGetHtmlDoc
    {
        public HtmlDocument GetHtmlDocument(HtmlDocument inputDocument)
        {
            var htmlString = 
                string.Join(
                    "", 
                    new List<string>{GetHead(inputDocument), GetPageContent(inputDocument)}
                        .Prepend("<!DOCTYPE html><html>")
                        .Append("</html>")
                    );
            HtmlDocument newDoc = new HtmlDocument();
            newDoc.LoadHtml(htmlString);
            return newDoc;
        }


        private string GetPageContent(HtmlDocument inputDocument)
        {
            var nodeStringList = new List<string>();
            var currentNode = 
                inputDocument.DocumentNode.SelectSingleNode("//*[@id='mw-content-text']/div[1]");
            while (currentNode != null)
            {
                nodeStringList.Add(currentNode.OuterHtml);
                currentNode = currentNode.NextSibling;
            }
            return string.Join("", nodeStringList.Prepend("<body>").Append("</body>"));
        }

        private string GetHead(HtmlDocument inputDocument)
        {
            var nodeStringList = new List<string>();
            var currentNode = inputDocument.DocumentNode.SelectSingleNode("//html/head").FirstChild;
            while (currentNode != null)
            {
                if ((currentNode.Name == "meta" &
                     currentNode.Attributes.Any(attribute => attribute.Name == "charset")) |
                    currentNode.Name == "title")
                {
                    nodeStringList.Add(currentNode.OuterHtml);
                }
                currentNode = currentNode.NextSibling;
            }
            return string.Join("", nodeStringList.Prepend("<head>").Append("</head>"));
        }

        public void Test()
        {
            var webGetter = new HtmlWeb();
            HtmlDocument doc = webGetter.Load("https://en.wikipedia.org/wiki/Far_future_in_fiction");

            Console.WriteLine(GetHead(doc));
            Console.WriteLine(GetPageContent(doc));





        }
    }
}