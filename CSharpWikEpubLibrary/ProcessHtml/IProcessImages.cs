using HtmlAgilityPack;
namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public interface IProcessImages
    {
        HtmlDocument ProcessDownloadLinks(HtmlDocument inputDocument, string imageDirectory);
    }
}