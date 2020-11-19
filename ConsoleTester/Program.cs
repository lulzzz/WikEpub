using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using WikEpubLib;
using WikEpubLib.Interfaces;
using System.Diagnostics;

namespace CSharpConsoleDebugger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<string> urls = new() { "https://en.wikipedia.org/wiki/Sean_Connery","https://en.wikipedia.org/wiki/Physiology", "https://en.wikipedia.org/wiki/YouTube" };
            string rootDirectory = @"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\TestFolder";
            string bookTitle = "TestBook1";
            Guid guid = Guid.NewGuid();

            HtmlInput htmlInput = new HtmlInput();
            ParseHtml parseHtml = new ParseHtml();
            GetWikiPageRecords getWikiPageRecords = new GetWikiPageRecords();
            GetXmlDocs getXmlDocs = new GetXmlDocs(new GetTocXml(), new GetContentXml(), new GetContainerXml());
            EpubOutput epubOutput = new EpubOutput(new HttpClient());

            HtmlsToEpub htmlsToEpub = new HtmlsToEpub(parseHtml, getWikiPageRecords, getXmlDocs, htmlInput, epubOutput);

            Stopwatch stopwatch = Stopwatch.StartNew();
            await htmlsToEpub.Transform(urls, rootDirectory, bookTitle, guid);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

        }

           }
}





