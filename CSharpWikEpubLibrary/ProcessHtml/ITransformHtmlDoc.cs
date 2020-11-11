using System.Collections.Generic;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public interface ITransformHtmlDoc
    {
        HtmlDocument Transform(HtmlDocument inputDocument);


    }
}