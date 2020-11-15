using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpWikEpubLibrary.FileManager
{
    public class Container : IContainer
    {
        public async Task CreateAsync(string rootDirectory)
        {
            var containerTask = CreateContainer();
            Directory.CreateDirectory(@$"{rootDirectory}META-INF\");
            await using Stream s = File.Create(@$"{rootDirectory}META-INF\container.xml");
            await containerTask.Result.SaveAsync(s, SaveOptions.None, CancellationToken.None);
        }

        public async Task<XDocument> CreateContainer() =>
            await Task.Run(() =>
            {
                XNamespace defaultNs = "urn:oasis:names:tc:opendocument:xmlns:container";
                return new XDocument(
                    new XElement(
                        defaultNs + "container",
                        new XElement(
                            defaultNs + "rootfiles",
                            new XElement(
                                defaultNs + "rootfile",
                                new XAttribute(
                                    "full-path", "OEBPS/content.opf"
                                ),
                                new XAttribute(
                                    "media-type", "application/oebps-package+xml"
                                )
                            )
                        )
                    )
                );
            });
        }
}