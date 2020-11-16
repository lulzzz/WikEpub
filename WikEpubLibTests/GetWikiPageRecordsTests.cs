using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikEpubLib;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WikEpubLibTests
{
    [TestClass]
    public class GetWikiPageRecordsTests
    {
        HtmlWeb webGet = new HtmlWeb();
        GetWikiPageRecords GetWikiPageRecords = new GetWikiPageRecords();
        string seanConnWiki = "https://en.wikipedia.org/wiki/Sean_Connery";
        string seanImageDir = "image_dir1";
        WikiPageRecord seanRecord;
        
        [TestInitialize]
        public void InitialiseTest()
        {
            var seanWikiDoc = webGet.Load(seanConnWiki);
            seanRecord = GetWikiPageRecords.GetRecordsFrom(seanWikiDoc, seanImageDir);
        }

        [TestMethod]
        public void Corrent_ID_Sean_Connery()
        {
            Assert.AreEqual("Sean_Connery", seanRecord.Id);
        }



        
    }

}
