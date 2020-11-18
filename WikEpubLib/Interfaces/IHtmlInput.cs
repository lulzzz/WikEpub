using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikEpubLib
{
    public interface IHtmlInput
    {
        Task<HtmlDocument[]> GetHtmlDocuments(IEnumerable<string> urls, HtmlWeb htmlWeb);
        
    }
}
