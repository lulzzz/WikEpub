using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public interface IParseHtmlDoc
    {
        Task<HtmlDocument> TransformAsync(HtmlDocument inputDocument);


    }
}