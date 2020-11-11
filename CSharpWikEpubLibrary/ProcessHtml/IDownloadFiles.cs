using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public interface IDownloadFiles
    {
        public void  DownloadAsync(IEnumerable<string> fromUrls, string toDirectory);
    }
}