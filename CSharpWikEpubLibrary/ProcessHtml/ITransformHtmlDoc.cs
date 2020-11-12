using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public interface ITransformHtmlDoc
    {
        HtmlDocument Transform(HtmlDocument inputDocument);


    }
}