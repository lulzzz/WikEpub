using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikEpubLib
{
    public interface IHtmlInput
    {
        Task<HtmlDocument[]> GetHtmlDocuments(IEnumerable<string> urls, HtmlWeb htmlWeb);
    }
}