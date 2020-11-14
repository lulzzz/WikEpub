using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpWikEpubLibrary.FileManager
{
    public interface IDownloadFiles
    {
        public Task  Download(IEnumerable<string> fromUrls, string toDirectory);
    }
}