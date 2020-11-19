using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikEpubLib.Records;

namespace WikEpubLib.Interfaces
{
    public interface IParseHtml
    {
        Task<HtmlDocument> ParseAsync(HtmlDocument htmlDocument, WikiPageRecord wikiPageRecord);

    }
}
