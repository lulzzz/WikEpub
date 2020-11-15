using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.FileManager
{
    public interface IToc
    {
        Task CreateAsync(Dictionary<HtmlDocument, string> htmlIds, string toDirectory, string bookTitle);
    }
}