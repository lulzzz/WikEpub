using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public interface IDownloadFiles
    {
        public void DownloadFiles(IEnumerable<string> fromUrl, string toDirectory);
    }
}