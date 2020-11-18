using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using WikEpubLib;
using WikEpubLib.Interfaces;

namespace CSharpConsoleDebugger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HtmlWeb webGet = new HtmlWeb();
            List<string> urls = new() { "https://en.wikipedia.org/wiki/Sean_Connery", "https://en.wikipedia.org/wiki/Physiology" };
            string rootDirectory = @"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\TestFolder";
            string bookTitle = "TestBook1";
            Guid guid = Guid.NewGuid();

            HtmlInput htmlInput = new HtmlInput();
            ParseHtml parseHtml = new ParseHtml();
            GetWikiPageRecords getWikiPageRecords = new GetWikiPageRecords();
            GetXmlDocs getXmlDocs = new GetXmlDocs(new GetTocXml(), new GetContentXml(), new GetContainerXml());
            EpubOutput epubOutput = new EpubOutput(new HttpClient());

            HtmlsToEpub htmlsToEpub = new HtmlsToEpub(parseHtml, getWikiPageRecords, getXmlDocs, htmlInput, epubOutput);

            await htmlsToEpub.Transform(urls, rootDirectory, bookTitle, guid);




        }
    }
}




