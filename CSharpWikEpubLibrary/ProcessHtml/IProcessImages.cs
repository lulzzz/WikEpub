using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public interface IProcessImages
    {
        Task<HtmlDocument> ProcessDownloadLinks(HtmlDocument inputDocument, string imageDirectory);
    }
}