//#define DL
//#define PRINT
#define LOG
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WikEpubLib.CreateDocs;
using WikEpubLib.Interfaces;
using WikEpubLib.Records;

namespace CSharpConsoleDebugger.Performance.DebugParseHtml
{
    public static class DebugHtmlParser
    {
        public static async Task DebugParser()
        {
#if LOG
            Console.WriteLine("Enter a log message");
            var userLogMessage = Console.ReadLine();
#endif
#if DL
            Console.WriteLine("Enter name for downloaded html file: ");
            var dlName = Console.ReadLine();
#endif
            HtmlWeb webGetter = new HtmlWeb();
            GetWikiPageRecords getRecords = new GetWikiPageRecords();
#if LOG
            var urls = new List<string>() { "https://en.wikipedia.org/wiki/Sean_Connery", "https://en.wikipedia.org/wiki/Physiology", "https://en.wikipedia.org/wiki/YouTube" };
#endif
#if DL

            var urls = new List<string>() { "https://en.wikipedia.org/wiki/Sean_Connery" };
#endif
#if PRINT
            var urls = new List<string>() { "https://en.wikipedia.org/wiki/Sean_Connery" };
#endif
            var docs = urls.Select(url => webGetter.Load(url)).ToList();
            IEnumerable<(HtmlDocument doc, WikiPageRecord record)> docRecs = docs.Select(doc => (doc, getRecords.From(doc, "image_repo")));
            Dictionary<string, IParseHtml> parserDict = new Dictionary<string, IParseHtml>() {
                {"new", new HtmlParser() },
                {"original", new ParseHtml()}
            };
            IParseHtml parseHtml = parserDict["new"];
            Stopwatch stopwatch = Stopwatch.StartNew();
            var parsedHtml = await Task.WhenAll(docRecs.Select(docRec => parseHtml.ParseAsync(docRec.doc, docRec.record)));
            stopwatch.Stop();
#if PRINT
            parsedHtml.ToList().ForEach(docRec => Console.WriteLine(docRec.doc.DocumentNode.OuterHtml));
#endif
#if DL
            parsedHtml.ToList().ForEach(html =>
            {
                using Stream stream = File.Create(@$"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\Performance\DebugParseHtml\HtmlProducts\{html.record.Id}_{dlName}.html");
                html.doc.Save(stream);
            });
#endif
#if LOG
            string timeMessage = $"run-time: {stopwatch.Elapsed.TotalSeconds} seconds";
            string logMessage = $"Most recent change: {userLogMessage} \n \n " +
                $"{timeMessage} \n \n " +
                $"------------------------------------------------- \n \n";
            File.AppendAllText(@"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\Performance\DebugParseHtml\ParseHtmlDebugLog.txt", logMessage);
#endif
        }
    }
}