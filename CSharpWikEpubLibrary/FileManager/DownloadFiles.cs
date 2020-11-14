using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSharpWikEpubLibrary.FileManager
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
        public async Task DownloadAsync(IEnumerable<string> fromUrls, string toDirectory)
        {
            await fromUrls.AsParallel().ToList().ForEachAsync(async url =>
            {
                var responseResult = _httpClient.GetAsync(url);
                await using var memoryStream = responseResult.Result.Content.ReadAsStreamAsync().Result;
                Directory.CreateDirectory(toDirectory);
                await using var fileStream = File.Create($"{toDirectory}{url.Split('/').LastOrDefault()}");
                await memoryStream.CopyToAsync(fileStream);
            });
        }

    }
}
