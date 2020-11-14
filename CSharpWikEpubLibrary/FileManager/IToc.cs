using System.Collections.Generic;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.FileManager
{
    public interface IToc
    {
        void Create(Dictionary<HtmlDocument, string> htmlIds, string toDirectory);
    }
}