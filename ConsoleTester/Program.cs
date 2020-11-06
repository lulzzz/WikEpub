using FSharp.Data;
using System;
using System.Linq;
using WikEpubLibrary;
namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {

            HtmlDocument htmlDocument = HtmlDocument.Load("https://en.wikipedia.org/wiki/Example_(musician)");
            //     Console.WriteLine(htmlDocument);
            var newDoc = ScrapeWiki.getNewHtml(htmlDocument);
            Console.Write(newDoc.ToString());
        }
    }
}


