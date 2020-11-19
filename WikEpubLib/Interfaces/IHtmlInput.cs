using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikEpubLib
{
    public interface IHtmlInput
    {
        Task<HtmlDocument[]> GetHtmlDocumentsFromAsync(IEnumerable<string> urls, HtmlWeb htmlWeb);
    }
}