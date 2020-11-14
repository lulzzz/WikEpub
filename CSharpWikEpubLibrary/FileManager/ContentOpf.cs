using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml.Linq;




namespace CSharpWikEpubLibrary.FileManager
{
    public class ContentOpf : IContentOpf
    {
        public async Task Create(Dictionary<HtmlDocument, string> htmlInfo, string inDirectory, string bookTitle)
        {
            var docTask =  GetXDocumentTask(htmlInfo, bookTitle);
            await using Stream s = File.Create(inDirectory + "content.opf");
            await docTask.Result.SaveAsync(s, SaveOptions.None, CancellationToken.None);
        }

        private async Task<XDocument> GetXDocumentTask(Dictionary<HtmlDocument, string> htmlInfo, string bookTitle)
        {
            return await Task.Run(() =>
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

                    XElement metadata =
                        new XElement(defaultNs + "metadata",
                            new XElement("{http://purl.org/dc/elements/1.1/}title", bookTitle, new XAttribute(xmlns, purl)),
                            new XElement("{http://purl.org/dc/elements/1.1/}publisher", "Wikipedia", new XAttribute(xmlns, purl)),
                            new XElement("{http://purl.org/dc/elements/1.1/}date",
                                $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}", new XAttribute(xmlns, purl)),
                            new XElement("{http://purl.org/dc/elements/1.1/}creator", "Harry Prior", new XAttribute(xmlns, purl))
                        );

                    XElement manifest =
                        new XElement(defaultNs + "manifest");

                    manifest.Add(
                        new XElement(
                            defaultNs + "item",
                            new XAttribute("id", "cover"),
                            new XAttribute("href", "cover.html"),
                            new XAttribute("media-type", "application/xhtml+xml")));

                    int imageId = 0;
                    string GetImageId() => $"image_{++imageId}";
                    htmlInfo.ToList().ForEach(ds =>
                    {
                        var imagePaths = GetLocalImagePaths(DocumentNodes(ds.Key));
                        manifest.Add(
                            new XElement(
                                defaultNs + "item",
                                new XAttribute("id", ds.Value),
                                new XAttribute("href", $"{ds.Value}.html"),
                                new XAttribute("media-type", "application/xhtml+xml")
                            )
                        );

                        if (imagePaths.Result.Count > 0)
                        {
                            imagePaths.Result.ForEach(path =>
                            {
                                manifest.Add(new XElement(
                                    defaultNs + "item",
                                    new XAttribute("id", GetImageId()),
                                    new XAttribute("href", path),
                                    new XAttribute("media-type", $"image/{path.Split('.').Last()}")
                                ));
                            });
                        }
                    });

                    XElement spine = new XElement(defaultNs + "spine", new XAttribute("toc", "ncxtoc"));
                    spine.Add(new XElement(defaultNs + "itemref", new XAttribute("idref", "cover")));
                    htmlInfo.ToList().ForEach(ds =>
                    {
                        spine.Add(new XElement(defaultNs + "itemref", new XAttribute("idref", ds.Value)));
                    });


                    package.Add(metadata);
                    package.Add(manifest);
                    package.Add(spine);

                    return new XDocument(new XDeclaration("1,0", "utf-8", "no"), package);
            });
        }

        IEnumerable<HtmlNode> DocumentNodes(HtmlDocument document) => document.DocumentNode.Descendants();


        private async Task<List<string>> GetLocalImagePaths(IEnumerable<HtmlNode> nodes) =>
            await Task.Run(() => nodes
                .Where(node => node.Name == "img")
                .Select(node => node.GetAttributeValue("src", "no_value")).Distinct()
                .Select(path => string.Join(@"\", path.Split(@"\")[^2..]))
                .ToList());
    }
}
