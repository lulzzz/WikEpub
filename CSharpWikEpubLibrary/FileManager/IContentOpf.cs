using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.FileManager
{
    public interface IContentOpf
    {
        public Task Create(Dictionary<HtmlDocument, string> htmlInfo, string directory, string bookTitle);

    }
}