using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikEpubLib.IO
{
    public class HtmlInput : IHtmlInput
    {
        public async Task<HtmlDocument[]> GetHtmlDocuments(IEnumerable<string> urls, HtmlWeb htmlWeb) =>
            await Task.WhenAll(urls.Select(url => htmlWeb.LoadFromWebAsync(url)));

    }
}
