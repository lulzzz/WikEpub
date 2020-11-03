using System;
using System.Collections.Generic;
using System.Text;

namespace Scraper.RecursiveWikiScraper
{
    interface IGetRecursiveHtml
    {
        IList<string> GetHtmlStrings();
    }
}
