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
        string imageDir = "image_dir1";
        WikiPageRecord seanRecord;
        WikiPageRecord physioRecord;
        WikiPageRecord physiologyRecord;
        WikiPageRecord paperRecord;
        WikiPageRecord markLawrenceRecord;

        List<WikiPageRecord> wikiPages;

        
        [TestInitialize]
        public void InitialiseTest()
        {
            var seanWikiDoc = webGet.Load("https://en.wikipedia.org/wiki/Sean_Connery");
            var physioWikiDoc = webGet.Load("https://en.wikipedia.org/wiki/Physical_therapy");
            var physiologyWikiDoc = webGet.Load("https://en.wikipedia.org/wiki/Physiology");
            var paperWikiDoc = webGet.Load("https://en.wikipedia.org/wiki/Page_(paper)");
            var markWikiDoc = webGet.Load("https://en.wikipedia.org/wiki/Mark_Lawrence_(cricketer)");
 
            seanRecord = GetWikiPageRecords.From(seanWikiDoc, imageDir);
            physioRecord = GetWikiPageRecords.From(physioWikiDoc, imageDir);
            physiologyRecord = GetWikiPageRecords.From(physiologyWikiDoc, imageDir);
            paperRecord = GetWikiPageRecords.From(paperWikiDoc, imageDir);
            markLawrenceRecord = GetWikiPageRecords.From(markWikiDoc, imageDir);

            wikiPages = new() { seanRecord, physioRecord, physiologyRecord, paperRecord, markLawrenceRecord};
        }

        [TestMethod]
        public void Correct_ID_Sean_Connery()
        {
            Assert.AreEqual("Sean_Connery", seanRecord.Id);
        }
        [TestMethod]
        public void Correct_ID_Paper()
        {
            Assert.AreEqual("Page_paper", paperRecord.Id);
        }
        [TestMethod]
        public void No_Pictures_Generates_Null_Value()
        {
            Assert.AreEqual(null, markLawrenceRecord.SrcMap);
        }

        [TestMethod]
        public void Correct_Src_Dict_Key_Connery()
        {
            List<string> keys = new ()
            {
                 "//upload.wikimedia.org/wikipedia/commons/thumb/0/0b/Sean_Connery_1964.png/220px-Sean_Connery_1964.png",
                "//upload.wikimedia.org/wikipedia/commons/thumb/2/26/Signature_of_Sean_Connery.svg/150px-Signature_of_Sean_Connery.svg.png",
                "//upload.wikimedia.org/wikipedia/commons/thumb/5/5f/Sean_Connery_plaque%2C_Fountainbridge_Edinburgh.jpg/220px-Sean_Connery_plaque%2C_Fountainbridge_Edinburgh.jpg",
                "//upload.wikimedia.org/wikipedia/commons/thumb/0/07/Lana_Turner_and_Sean_Connery_%E2%80%94_Another_Time%2C_Another_Place.jpg/170px-Lana_Turner_and_Sean_Connery_%E2%80%94_Another_Time%2C_Another_Place.jpg",
                "//upload.wikimedia.org/wikipedia/commons/thumb/1/13/ETH-BIB_Goldfinger_1964_%E2%80%93_Com_C13-035-007.jpg/220px-ETH-BIB_Goldfinger_1964_%E2%80%93_Com_C13-035-007.jpg",
                "//upload.wikimedia.org/wikipedia/commons/thumb/f/fa/Sean_Connery_as_James_Bond_%281971%29.jpg/220px-Sean_Connery_as_James_Bond_%281971%29.jpg",
                "//upload.wikimedia.org/wikipedia/commons/thumb/1/14/Tippi_Hedren_and_Sean_Connery_in_%22Marnie%22_%281964%29.png/220px-Tippi_Hedren_and_Sean_Connery_in_%22Marnie%22_%281964%29.png",
                "//upload.wikimedia.org/wikipedia/commons/thumb/4/40/Hepburn_Connery_Robin_and_Marian_Still_1976.jpg/220px-Hepburn_Connery_Robin_and_Marian_Still_1976.jpg",
                "//upload.wikimedia.org/wikipedia/commons/thumb/c/c5/SeanConnery88.jpg/170px-SeanConnery88.jpg",
                "//upload.wikimedia.org/wikipedia/commons/thumb/c/c8/SeanConneryJune08.jpg/170px-SeanConneryJune08.jpg",
                "//upload.wikimedia.org/wikipedia/commons/thumb/6/66/Diane_Cilento%2C_1954.jpg/170px-Diane_Cilento%2C_1954.jpg",
                "//upload.wikimedia.org/wikipedia/commons/thumb/e/e5/ConneryKilt.jpg/180px-ConneryKilt.jpg",
                "//upload.wikimedia.org/wikipedia/en/thumb/4/4a/Commons-logo.svg/30px-Commons-logo.svg.png",
                "//upload.wikimedia.org/wikipedia/commons/thumb/f/fa/Wikiquote-logo.svg/34px-Wikiquote-logo.svg.png",
                "//upload.wikimedia.org/wikipedia/en/thumb/8/8a/OOjs_UI_icon_edit-ltr-progressive.svg/10px-OOjs_UI_icon_edit-ltr-progressive.svg.png",
                "//upload.wikimedia.org/wikipedia/commons/thumb/8/89/Symbol_book_class2.svg/16px-Symbol_book_class2.svg.png",
                "//upload.wikimedia.org/wikipedia/en/thumb/4/48/Folder_Hexagonal_Icon.svg/16px-Folder_Hexagonal_Icon.svg.png",
            };
            keys.ForEach(key =>
            {
                Assert.IsTrue(seanRecord.SrcMap.ContainsKey(key));
                Assert.AreEqual(seanRecord.SrcMap.Count, keys.Count);
           });
        }
        
        
        //TODO make these general purpose so they test every html page in a list
        [TestMethod]
        public void Src_Dict_Value_Starts_With_Correct_Value()
        {
            wikiPages.ForEach(page =>
            {
                seanRecord.SrcMap.ToList().ForEach(t =>
                {
                    Assert.IsTrue(t.Value.StartsWith("image_"));
                });
            });
       }
        
        [TestMethod]
        public void Src_Dict_Value_Ends_With_Correct_FileType()
        {
            HashSet<string> fTypes = new () { ".png", ".jpeg", ".jpg", ".svg", ".apng", ".avif", ".gif", ".jfif", ".pjpeg", ".pjp", ".webp" };

            wikiPages.ForEach(page =>
            {
                page.SrcMap.ToList().ForEach(t =>
                {
                   Assert.IsTrue(fTypes.Any(type => t.Value.EndsWith(type)));
                });

            });
            
        }

        [TestMethod]
        public void Src_Dict_Check_Value_Numeric_Between_Start_And_End()
        {
            wikiPages.ForEach(page =>
            {
                page.SrcMap.ToList().ForEach(t =>
                {
                    Assert.IsTrue(int.TryParse(t.Value.Split('\\')[1].Split('.')[0].Replace("image_", ""), out _));
                });

            });
        }       
    }
}
