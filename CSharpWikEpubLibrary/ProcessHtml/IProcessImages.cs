using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public interface IProcessImages
    {
        HtmlDocument ProcessDownloadLinks(HtmlDocument inputDocument, string imageDirectory);
    }
}