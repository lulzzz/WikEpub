using HtmlAgilityPack;
namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public interface IProcessPictureDownloadLinks
    {
        HtmlDocument ProcessDownloadLinks(HtmlDocument inputDocument);
    }
}