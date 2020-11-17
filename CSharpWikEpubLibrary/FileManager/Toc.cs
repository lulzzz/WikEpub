using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpWikEpubLibrary.FileManager
{
    public class Toc : IToc
    {
        public async Task CreateAsync(Dictionary<HtmlDocument, string> htmlIds, string toDirectory, string bookTitle)
        {
            var tocDocTask = GetTocDocument(htmlIds, bookTitle);
            Directory.CreateDirectory(toDirectory);
            await using Stream s = File.Create($"{toDirectory}toc.ncx");
            await tocDocTask.Result.SaveAsync(s, SaveOptions.None, CancellationToken.None);
        }

        public async Task<XDocument> GetTocDocument(Dictionary<HtmlDocument, string> htmlIds, string bookTitle)
        {
            return await Task.Run(() =>
            {
                string pOrder = "playOrder";
                string navPoint = "navPoint";
                string navLabel = "navLabel";
                string content = "content";

                int playOrder = 1;
                string GetPlayOrder() => (playOrder++).ToString();

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
                                )
                            )
                    );

                XElement docTitle = new XElement(
                    defaultNs + "docTitle",
                    new XElement(defaultNs + "text", bookTitle)
                    );

                XElement navMap = new XElement(defaultNs + "navMap");

                htmlIds.ToList().ForEach(ds =>
                {
                    IEnumerable<HtmlNode> nodes = GetDescendants(ds.Key);
                    var title = GetTitle(nodes);
                    var sections = GetSectionHeaders(nodes);

                    XElement docNavPoint = new XElement(
                        defaultNs + navPoint,
                        new XAttribute("id", ds.Value),
                        new XAttribute(pOrder, GetPlayOrder()),
                        new XElement(
                            defaultNs + navLabel,
                            new XElement(
                               defaultNs + "text", title.Replace('_', ' '))
                            ),
                        new XElement(defaultNs + content, new XAttribute("src", $"{ds.Value}.html"))
                    );
                    sections.ToList().ForEach(s =>
                    {
                        XElement section = new XElement(
                            defaultNs + navPoint,
                            new XAttribute("id", ds.Value),
                            new XAttribute(pOrder, GetPlayOrder()),
                            new XElement(
                                defaultNs + navLabel,
                                new XElement(
                                    defaultNs + "text", s.Item1)
                            ),
                            new XElement(defaultNs + content, new XAttribute("src", $"{ds.Value}.html{s.Item2}"))
                        );
                        docNavPoint.Add(section);
                    });
                    navMap.Add(docNavPoint);
                });
                ncx.Add(docTitle);
                ncx.Add(navMap);
                return new XDocument(ncx);
            });
        }

        private IEnumerable<HtmlNode> GetDescendants(HtmlDocument doc) =>
            doc.DocumentNode.Descendants();

        private string GetTitle(IEnumerable<HtmlNode> nodes) =>
            nodes
                .First(node => node.Name == "title")
                .InnerHtml
                .Split('-')
                .First()
                .Trim()
                .Replace(' ', '_');

        private IEnumerable<(string, string)> GetSectionHeaders(IEnumerable<HtmlNode> nodes) =>
            nodes.Where(n => n.Name == "h2")
                .Select(n => n.FirstChild)
                .Select(n => (n.InnerHtml, $"#{n.GetAttributeValue("id", "none")}"));
    }
}