using CSharpConsoleDebugger.Performance.DebugMainConversion;
using CSharpConsoleDebugger.Performance.DebugParseHtml;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WikEpubLib;
using WikEpubLib.CreateDocs;
using WikEpubLib.IO;
using WikEpubLib.Records;

namespace CSharpConsoleDebugger
{
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            //AnalysePerf.GetRunTime();
            await CreateEpub();
            //await DebugHtmlParser.DebugParser();
            
        }

        private static async Task CreateEpub()
        {
            List<string> urls = new() {
                "https://en.wikipedia.org/wiki/Sean_Connery", 
                "https://en.wikipedia.org/wiki/Physiology", 
                "https://en.wikipedia.org/wiki/YouTube",
                "https://en.wikipedia.org/wiki/Eukaryote",
                "https://en.wikipedia.org/wiki/Cell_(biology)",
                "https://en.wikipedia.org/wiki/Matrix_(mathematics)",
                "https://en.wikipedia.org/wiki/Mathematics",
                "https://en.wikipedia.org/wiki/Computer_science",
                "https://en.wikipedia.org/wiki/Automata_theory",
                "https://en.wikipedia.org/wiki/Computational_problem"
            };
            string rootDirectory = @"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\TestFolder";
            string bookTitle = "TestBook1";
            Guid guid = Guid.NewGuid();

            HtmlInput htmlInput = new HtmlInput();
            HtmlParser parseHtml = new HtmlParser();
            GetWikiPageRecords getWikiPageRecords = new GetWikiPageRecords();
            GetXmlDocs getXmlDocs = new GetXmlDocs(new GetTocXml(), new GetContentXml(), new GetContainerXml());
            EpubOutput epubOutput = new EpubOutput(new HttpClient());

            GetEpub getEpub = new GetEpub(parseHtml, getWikiPageRecords, getXmlDocs, htmlInput, epubOutput);

            await getEpub.FromAsync(urls, rootDirectory, bookTitle, guid);
        }
    }
}