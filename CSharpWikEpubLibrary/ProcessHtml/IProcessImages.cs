using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public interface IProcessImages
    {
        Task<HtmlDocument> ProcessImageDownloadsAsync(HtmlDocument inputDocument, string imageDirectory);
    }
}