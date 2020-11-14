using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace CSharpWikEpubLibrary.ProcessHtml
{
    public interface IConvertEpub
    {
        Task ConvertAsync(IEnumerable<string> urls, string rootDirectory);
    }
}