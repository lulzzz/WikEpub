using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WikEpubLib.Interfaces;

namespace WikEpubLib.CreateDocs
{
    public class GetContentXml : IGetContentXml
    {
        public XDocument From(IEnumerable<WikiPageRecord> wikiPageRecords, string bookTitle)
        {
            XNamespace defaultNs = "http://www.idpf.org/2007/opf";

            XElement package =
                new XElement(
                    defaultNs + "package",
                    new XAttribute("version", "2.0"),
                    new XAttribute("unique-identifier", "bookid")
                );

            var xmlns = XNamespace.Xmlns + "dc";
            XNamespace purl = "http://purl.org/dc/elements/1.1/";

            // TODO redo default namespace for this seciton without repeates
            XElement metadata =
                new XElement(defaultNs + "metadata",
                    new XElement(purl + "title", bookTitle, new XAttribute(xmlns, purl)),
                    new XElement(purl + "publisher", "Wikipedia", new XAttribute(xmlns, purl)),
                    new XElement(purl + "date",
                        $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}", new XAttribute(xmlns, purl)),
                    new XElement(purl + "creator", "Harry Prior", new XAttribute(xmlns, purl)),
                    new XElement(defaultNs + "meta", new XAttribute("name", "cover"), new XAttribute("content", "cover-image"))
                );

            XElement manifest =
                        new XElement(defaultNs + "manifest");

            manifest.Add(itemElement(defaultNs, "cover", "cover.html", "application/xhtml+xml"));

            manifest.Add(itemElement(defaultNs, "ncxtoc", "toc.ncx", "application/dtbncx+xml"));

            int imageId = 1;
            string GetImageId() => $"image_{imageId++}";
            foreach (var record in wikiPageRecords)
            {
                manifest.Add(itemElement(defaultNs, record.Id, $"{record.Id}.html", "application/xhtml+xml"));
                if (record.SrcMap is not null)
                {
                    foreach (var dictItem in record.SrcMap)
                    {
                        manifest.Add(itemElement(defaultNs, GetImageId(), dictItem.Value, $"image/{dictItem.Value.Split('.').Last().ToLower()}"));
                    }
                }
            }

            XElement spine = new XElement(defaultNs + "spine", new XAttribute("toc", "ncxtoc"));
            spine.Add(new XElement(defaultNs + "itemref", new XAttribute("idref", "cover")));
            foreach (var record in wikiPageRecords)
            {
                spine.Add(new XElement(defaultNs + "itemref", new XAttribute("idref", record.Id)));
            }
            package.Add(metadata);
            package.Add(manifest);
            package.Add(spine);

            return new XDocument(new XDeclaration("1,0", "utf-8", "no"), package);
        }

        private XElement itemElement(XNamespace defaultNs, string id, string href, string mediaType) =>
           new XElement(
               defaultNs + "item",
               new XAttribute("id", id),
               new XAttribute("href", href),
               new XAttribute("media-type", mediaType)
               );
    }
}