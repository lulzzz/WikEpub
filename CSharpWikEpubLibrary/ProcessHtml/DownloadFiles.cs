using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CSharpWikEpubLibrary.ScrapeWiki;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public class DownloadFiles : IDownloadFiles
    {
        private readonly HttpClient _httpClient;

        public DownloadFiles(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Download collection of urls in parallel to specific directory.
        /// </summary>
        /// <remarks>
        /// Urls should start with 'https:'. Directories should end with '/'
        /// </remarks>
        /// <param name="fromUrls">Urls to download from</param>
        /// <param name="toDirectory">Root directory where files will be downloaded to</param>
        public void DownloadAsync(IEnumerable<string> fromUrls, string toDirectory) =>
            fromUrls.AsParallel().ToList().ForEach(url =>
            {
                var responseResult = _httpClient.GetAsync(url);
                using var memoryStream = responseResult.Result.Content.ReadAsStreamAsync().Result;
                using var fileStream = File.Create($"{toDirectory}{url.Split('/').LastOrDefault()}"); 
                memoryStream.CopyToAsync(fileStream);
            });
        

    }
}
