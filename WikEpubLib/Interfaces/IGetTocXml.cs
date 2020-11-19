﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Records;

namespace WikEpubLib.Interfaces
{
    public interface IGetTocXml
    {
        XDocument From(IEnumerable<WikiPageRecord> pageRecords, string bookTitle);
    }
}
