using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikEpubLib.Records;

namespace WikEpubLib.Interfaces
{
    public interface IGetWikiPageRecords
    {
        WikiPageRecord From(HtmlDocument html, string imageDirectory);
    }
}
