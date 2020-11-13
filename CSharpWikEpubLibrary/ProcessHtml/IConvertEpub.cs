using System;
using System.Collections.Generic;
using HtmlAgilityPack;
namespace CSharpWikEpubLibrary.ProcessHtml
{
    public interface IConvertEpub
    {
        void Convert(IEnumerable<string> urls, string toDirectory);
    }
}