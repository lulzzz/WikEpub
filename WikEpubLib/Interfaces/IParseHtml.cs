using HtmlAgilityPack;
using System.Threading.Tasks;
using WikEpubLib.Records;

namespace WikEpubLib.Interfaces
{
    public interface IParseHtml
    {
        Task<HtmlDocument> ParseAsync(HtmlDocument htmlDocument, WikiPageRecord wikiPageRecord);
    }
}