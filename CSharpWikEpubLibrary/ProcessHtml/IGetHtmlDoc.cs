using System.Collections.Generic;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public interface IGetHtmlDoc
    {
        HtmlDocument GetHtmlDocument(HtmlDocument inputDocument);


    }
}