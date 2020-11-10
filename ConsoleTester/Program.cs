using System;
using CSharpWikEpubLibrary.ScrapeWiki;
using HtmlAgilityPack;

namespace CSharpConsoleDebugger
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlDoc scrape = new GetHtmlDoc();

            var webGetter = new HtmlWeb();
            HtmlDocument doc = webGetter.Load("https://en.wikipedia.org/wiki/Far_future_in_fiction");

            var newDoc = scrape.GetHtmlDocument(doc);

            Console.WriteLine(newDoc.DocumentNode.OuterHtml);
        }
    }
}


