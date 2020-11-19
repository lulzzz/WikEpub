using HtmlAgilityPack;
using WikEpubLib.Records;

namespace WikEpubLib.Interfaces
{
    public interface IGetWikiPageRecords
    {
        WikiPageRecord From(HtmlDocument html, string imageDirectory);
    }
}