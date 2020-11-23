using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikEpubLib.IO
{
    public class HtmlInput : IHtmlInput
    {
        public async Task<HtmlDocument[]> GetHtmlDocumentsFromAsync(IEnumerable<string> urls, HtmlWeb htmlWeb) =>
            await Task.WhenAll(urls.Select(url => htmlWeb.LoadFromWebAsync(TranslateToApiCall(url))));

        private string TranslateToApiCall(string url) =>
            $@"https://en.wikipedia.org/api/rest_v1/page/html/{url.Split('/').Last()}";
    }
}