using System.Collections.Generic;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.FileManager
{
    public interface IContentOpf
    {
        public void Create(Dictionary<HtmlDocument, string> htmlInfo, string directory, string bookTitle);

    }
}