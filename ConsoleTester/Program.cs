using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CSharpWikEpubLibrary.FileManager;
using CSharpWikEpubLibrary.ProcessHtml;
using FSharp.Data;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace CSharpConsoleDebugger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var web = new HtmlWeb();
            var doc = web.Load("https://en.wikipedia.org/wiki/Sean_Connery");
            IParseHtmlDoc getEpub = new ParseHtml();
            var epubDoc = getEpub.Transform(doc);
            //Console.WriteLine(epubDoc.DocumentNode.SelectSingleNode("//html").OuterHtml + "\n");

            //using HttpClient httpClient = new HttpClient();
            //IProcessImages images = new ProcessImages(new DownloadFiles(httpClient ));
            //var processedDoc =  await images.ProcessDownloadLinks(epubDoc, @"C:\Users\User\Documents\Code\WikEpub\CSharpWikEpubLibrary\ProcessHtml\TestDlFolder\");
            
            //Console.WriteLine(processedDoc.DocumentNode.SelectSingleNode("/").OuterHtml);

            Dictionary<HtmlDocument, string> idDict = new Dictionary<HtmlDocument, string>()
            {
                {epubDoc, "doc_1"}
            };

            //IContentOpf contentOpf = new ContentOpf();
            //var getContentTask =  contentOpf.Create(idDict, @"C:\Users\User\Documents\Code\WikEpub\ConsoleTester\TestFolder\", "HarrysBook");

            // do cool stuff here
            //await getContentTask;
        }

        private static void httpClientTest()
        {
            HttpClient httpClient = new HttpClient();
            IDownloadFiles dlFiles = new DownloadFiles(httpClient);
            IEnumerable<string> urls = new List<string>
            {
                "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0b/Sean_Connery_1964.png/220px-Sean_Connery_1964.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/0/07/Lana_Turner_and_Sean_Connery_%E2%80%94_Another_Time%2C_Another_Place.jpg/170px-Lana_Turner_and_Sean_Connery_%E2%80%94_Another_Time%2C_Another_Place.jpg",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/1/13/ETH-BIB_Goldfinger_1964_%E2%80%93_Com_C13-035-007.jpg/220px-ETH-BIB_Goldfinger_1964_%E2%80%93_Com_C13-035-007.jpg",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fa/Sean_Connery_as_James_Bond_%281971%29.jpg/220px-Sean_Connery_as_James_Bond_%281971%29.jpg",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/1/14/Tippi_Hedren_and_Sean_Connery_in_%22Marnie%22_%281964%29.png/220px-Tippi_Hedren_and_Sean_Connery_in_%22Marnie%22_%281964%29.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Hepburn_Connery_Robin_and_Marian_Still_1976.jpg/220px-Hepburn_Connery_Robin_and_Marian_Still_1976.jpg",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c5/SeanConnery88.jpg/170px-SeanConnery88.jpg",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c8/SeanConneryJune08.jpg/170px-SeanConneryJune08.jpg",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/6/66/Diane_Cilento%2C_1954.jpg/170px-Diane_Cilento%2C_1954.jpg",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e5/ConneryKilt.jpg/180px-ConneryKilt.jpg"
            };
            Console.WriteLine("Starting Dowloads");
            dlFiles.DownloadAsync(urls, @"C:\Users\User\Documents\Code\WikEpub\CSharpWikEpubLibrary\ProcessHtml\TestDlFolder\");
            Console.Write("Finished!!!");
        }
    }
}



