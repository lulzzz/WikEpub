#define DL 
#define LOG
using WikEpubLib.Records;
using HtmlAgilityPack;
using WikEpubLib.CreateDocs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CSharpConsoleDebugger.Performance.DebugParseHtml
{
    public static class DebugHtmlParser
    {

        public static async Task DebugParser()
        {
            Console.WriteLine("Enter a log message");
            var userLogMessage = Console.ReadLine();
            HtmlWeb webGetter = new HtmlWeb();
            GetWikiPageRecords getRecords = new GetWikiPageRecords(); 
           
            var urls = new List<string>() { "https://en.wikipedia.org/wiki/Sean_Connery", "https://en.wikipedia.org/wiki/Physiology", "https://en.wikipedia.org/wiki/YouTube" };
            var docs = urls.Select(url => webGetter.Load(url)).ToList();
            var docRecs = docs.Select(doc => (doc, getRecords.From(doc, "image_repo")));
            

            ParseHtml parseHtml = new ParseHtml();

            Stopwatch stopwatch = Stopwatch.StartNew();

            var parsedHtml = await Task.WhenAll(docRecs.Select(docRec => parseHtml.ParseAsync(docRec.doc, docRec.Item2)));

            stopwatch.Stop();

            string timeMessage = $"run-time: {stopwatch.Elapsed.TotalSeconds} seconds";
#if DL
            parsedHtml.ToList().ForEach(html => {
                using Stream stream = File.Create(@$"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\Performance\DebugParseHtml\HtmlProducts\{html.record.Id}.html");
                html.doc.Save(stream);
            });
#endif
#if LOG

            string logMessage = $"Most recent change: {userLogMessage} \n \n " +
                $"{timeMessage} \n \n " +
                $"-------------------------------------------------";
            File.AppendAllText(@"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\Performance\DebugParseHtml\ParseHtmlDebugLog.txt", logMessage)
#endif 




            



           
            

        }
        
        

    }
}
