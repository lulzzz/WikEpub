using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public interface IParseHtmlDoc
    {
        HtmlDocument Transform(HtmlDocument inputDocument);


    }
}