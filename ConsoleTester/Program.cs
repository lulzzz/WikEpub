using FSharp.Data;
using Microsoft.FSharp.Core;
using Microsoft.VisualBasic;
using System;
using WebScraper;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlDocument doc = HtmlDocument.Load("https://www.google.com");
            Console.WriteLine(doc.Body());
            Say.hello("Harry");
            Console.WriteLine("Hello World!");
        }
    }
}


