using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using CSharpWikEpubLibrary.FileManager;
using CSharpWikEpubLibrary.ProcessHtml;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using WikEpubLib;

namespace CSharpConsoleDebugger
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlWeb webGet = new HtmlWeb();
            var html = webGet.Load("https://en.wikipedia.org/wiki/Sean_Connery");
            var html2 = webGet.Load("https://en.wikipedia.org/wiki/Physiology");


            GetWikiPageRecords getPageRecord = new();
            var record = getPageRecord.From(html, "image_dir");
            var record2 = getPageRecord.From(html2, "image_dir");

            GetContentXml getContentOpf = new GetContentXml();
            GetTocXml getToxXml = new GetTocXml();

            var testXml = getContentOpf.From(new List<WikiPageRecord> { record , record2}, "TestBook");
            var tocXml = getToxXml.From(new List<WikiPageRecord> { record, record2 },"TestBook");

            Console.WriteLine(tocXml.ToString());



        }
    }
}




