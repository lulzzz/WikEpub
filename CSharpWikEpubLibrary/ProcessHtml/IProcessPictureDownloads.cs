using HtmlAgilityPack;
namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public interface IProcessPictureDownloads
    {
        HtmlDocument ProcessDownloadLinks(HtmlDocument inputDocument);
    }
}