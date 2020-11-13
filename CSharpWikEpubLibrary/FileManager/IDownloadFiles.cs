using System.Collections.Generic;

namespace CSharpWikEpubLibrary.FileManager
{
    public interface IDownloadFiles
    {
        public void  Download(IEnumerable<string> fromUrls, string toDirectory);
    }
}