using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikEpubLib
{
    interface IParseHtml
    {
        public HtmlDocument Parse(HtmlDocument htmlDocument, WikiPageRecord wikiPageRecord);

    }
}
