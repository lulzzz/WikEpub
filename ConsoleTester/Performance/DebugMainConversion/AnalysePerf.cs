using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WikEpubLib;
using WikEpubLib.CreateDocs;
using WikEpubLib.IO;
using WikEpubLib.Records;

namespace CSharpConsoleDebugger.Performance.DebugMainConversion
{
    public static class AnalysePerf
    {
        public static void GetRunTime()
        {
            try
            {
                Console.WriteLine("Enter a log message: ");
                var userLogMessage = Console.ReadLine();

                List<string> urls = new() { "https://en.wikipedia.org/wiki/Sean_Connery", "https://en.wikipedia.org/wiki/Physiology", "https://en.wikipedia.org/wiki/YouTube" };
                string rootDirectory = @"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\Performance\DebugMainConversion\EpubRepo";
                string bookTitle = "TestBook1";

                Directory.CreateDirectory(@"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\Performance\DebugMainConversion\EpubRepo");
                int num_iterations = 20;
                var avg_time = Enumerable.Range(0, num_iterations).Sum(x =>
                {
                    GetEpub getEpub = GetEpubClass();
                    return TimeCreateBook(getEpub, urls, rootDirectory, bookTitle).Result;
                }) / num_iterations;

                string logMessage = $"{DateTime.Now} \n \n " +
                    $"Most recent change: {userLogMessage} \n \n " +
                    $"average run-time over {num_iterations} iterations: {Math.Round(avg_time, 3)} seconds \n \n" +
                    $"---------------------------------------------- " +
                    $"\n \n";
                File.AppendAllText(@"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\Performance\DebugMainConversion\RunTimeLog.txt", logMessage);
            }
            catch (Exception e)
            {
                var exceptionMessage = $"Exception occured: \n \n " +
                    $"{e.Message} \n \n " +
                    $"---------------------------------------------- ";
                File.AppendAllText(@"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\Performance\DebugMainConversion\RunTimeLog.txt", exceptionMessage);
            }
            finally
            {
                Directory.Delete(@"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\Performance\DebugMainConversion\EpubRepo", true);
            }
        }

        private static GetEpub GetEpubClass()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            HtmlInput htmlInput = new HtmlInput();
            HtmlParser parseHtml = new HtmlParser();
            GetWikiPageRecords getWikiPageRecords = new GetWikiPageRecords();
            GetXmlDocs getXmlDocs = new GetXmlDocs(new GetTocXml(), new GetContentXml(), new GetContainerXml());
            EpubOutput epubOutput = new EpubOutput(new HttpClient());

            GetEpub getEpub = new GetEpub(parseHtml, getWikiPageRecords, getXmlDocs, htmlInput, epubOutput);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalSeconds);
            return getEpub;
        }

        private static async Task<double> TimeCreateBook(GetEpub getEpub, List<string> urls, string directory, string bookTitle)
        {
            Guid guid = Guid.NewGuid();
            Stopwatch stopwatch = Stopwatch.StartNew();
            await getEpub.FromAsync(urls, directory, bookTitle, guid);
            stopwatch.Stop();
            File.Delete(@$"{directory}\{guid}.epub");
            return stopwatch.Elapsed.TotalSeconds;
        }
    }
}