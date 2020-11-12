using System.Collections.Generic;

namespace CSharpWikEpubLibrary.FileManager
{
    public interface IDownloadFiles
    {
        public void  DownloadAsync(IEnumerable<string> fromUrls, string toDirectory);
    }
}