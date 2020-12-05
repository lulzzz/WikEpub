using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikEpubLib.Records;

namespace WikEpubLibTests
{
    [TestClass]
    public class GetWikiPageRecordsTests
    {
        private HtmlWeb webGet = new HtmlWeb();
        private GetWikiPageRecords GetWikiPageRecords = new GetWikiPageRecords();
        private string imageDir = "image_dir";
        private WikiPageRecord seanRecord;
        private WikiPageRecord physioRecord;
        private WikiPageRecord physiologyRecord;
        private WikiPageRecord paperRecord;
        private WikiPageRecord markLawrenceRecord;

        private List<WikiPageRecord> wikiPages;

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
            paperRecord =  GetWikiPageRecords.From(paperWikiDoc, imageDir);
            markLawrenceRecord = GetWikiPageRecords.From(markWikiDoc, imageDir);

            wikiPages = new() { seanRecord, physioRecord, physiologyRecord, paperRecord, markLawrenceRecord };
            //wikiPages.ForEach(page => page.SrcMap.ToList().ForEach(item => Console.WriteLine($"{item.Key} -- {item.Value}")));
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
        public void Src_Dict_Value_Starts_With_Correct_Value()
        {
            wikiPages.ForEach(page =>
            {
                if (page.SrcMap is not null)
                    page.SrcMap.ToList().ForEach(t =>
                    {
                        Assert.IsTrue(t.Value.StartsWith("image_"));
                    });
            });
        }

        [TestMethod]
        public void Src_Dict_Value_Ends_With_Correct_FileType()
        {
            HashSet<string> fTypes = new() { ".png", ".jpeg", ".jpg", ".svg", ".apng", ".avif", ".gif", ".jfif", ".pjpeg", ".pjp", ".webp" };

            wikiPages.ForEach(page =>
            {
                if (page.SrcMap is not null)
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
                if (page.SrcMap is not null)
                {
                    page.SrcMap.ToList().ForEach(t =>
                    Assert.AreEqual(true, int.TryParse(t.Value.Split('\\')[1].Split('.')[0].Replace("image_", string.Empty).Trim(), out _)));
                }
            });
        }
        [TestMethod]
        public void Access_Record_Multiple_Times_Same_Values_Src()
        {
            var t1 = seanRecord.SrcMap.Select(v => v.Value);
            var t2 = seanRecord.SrcMap.Select(v => v.Value);

            t1.Zip(t2).ToList().ForEach(t => Assert.AreEqual(t.First, t.Second));

        }
    }
}