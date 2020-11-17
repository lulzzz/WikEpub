using System.Collections.Generic;
using System.Xml.Linq;
using WikEpubLib.Interfaces;

namespace WikEpubLib
{
    public class GetTocXml : IGetTocXml
    {
        public XDocument From(IEnumerable<WikiPageRecord> pageRecords, string bookTitle)
        {
            XNamespace defaultNs = "http://www.daisy.org/z3986/2005/ncx/";

            XElement ncx = new XElement(
                defaultNs + "ncx",
                new XAttribute("version", "2005-1"),
                new XElement(
                        defaultNs + "head",
                        new XElement(
                            defaultNs + "meta",
                            new XAttribute("name", "cover"),
                            new XAttribute("content", "cover")
                            )));

            XElement docTitle = new XElement(
                defaultNs + "docTitle",
                new XElement(defaultNs + "text", bookTitle)
                );

            ncx.Add(docTitle);
            ncx.Add(GetNavMap(pageRecords, defaultNs));
            return new XDocument(ncx);
        }

        private XElement GetNavMap(IEnumerable<WikiPageRecord> pageRecords, XNamespace defaultNs)
        {
            int playOrder = 1;
            string GetPlayOrder() => (playOrder++).ToString();
            XElement navMap = new XElement(defaultNs + "navMap");

            foreach (var record in pageRecords)
            {
                XElement docNavPoint = GetNavPoint(defaultNs, record.Id, GetPlayOrder());
                foreach (var recordSection in record.SectionHeadings)
                {
                    XElement section = GetNavPoint(defaultNs, record.Id, GetPlayOrder(), recordSection.sectionName, recordSection.id);
                    docNavPoint.Add(section);
                }
                navMap.Add(docNavPoint);
            }
            return navMap;
        }

        private XElement GetNavPoint(XNamespace defaultNs, string id, string playOrder) =>
            new XElement(
                defaultNs + "navPoint",
                new XAttribute("id", id),
                new XAttribute("playOrder", playOrder),
                new XElement(defaultNs + "navLabel", new XElement(defaultNs + "text", id.Replace('_', ' '))),
                new XElement(defaultNs + "content", new XAttribute("src", $"{id}.html"))
                );

        private XElement GetNavPoint(XNamespace defaultNs, string id, string playOrder, string sectionName, string hashId) =>
            new XElement(
                defaultNs + "navPoint",
                new XAttribute("id", id),
                new XAttribute("playOrder", playOrder),
                new XElement(defaultNs + "naveLabel", new XElement(defaultNs + "text", sectionName)),
                new XElement(defaultNs + "content", new XAttribute("src", $"{id}.html{(hashId == "#null" ? string.Empty : hashId)}"))
                );
    }
}