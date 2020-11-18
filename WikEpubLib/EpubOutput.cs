using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WikEpubLib
{
    public class EpubOutput : IEpubOutput
    {
        public async Task CreateDirectories(string rootDirectory, Guid folderID) =>
            await Task.Run(() =>
            {
                Directory.CreateDirectory(@$"{rootDirectory}\{folderID}\OEBPS");
                Directory.CreateDirectory(@$"{rootDirectory}\{folderID}\META-INF");
                Console.WriteLine("Directories Created");
            });

        public async Task SaveToAsync(string rootDirectory, Guid folderId, IEnumerable<(XmlType type, XDocument doc)> xmlDocs, 
            IEnumerable<(HtmlDocument doc, WikiPageRecord record)> htmlDocs)
        {
            var directories = GetDirectoryDict(rootDirectory, folderId);
            await Task.WhenAll(
                xmlDocs.Select(t => t.type switch
                {
                    XmlType.Container => SaveTaskAsync(t.doc, directories[Directories.METAINF], "container.xml"),
                    XmlType.Content => SaveTaskAsync(t.doc, directories[Directories.OEBPS], "content.opf"),
                    XmlType.Toc => SaveTaskAsync(t.doc, directories[Directories.OEBPS], "toc.ncx"),
                    _ => throw new ArgumentException("Unknown XML type found in xml switch expression")

                }).Concat(htmlDocs.Select(t => SaveTaskAsync(t.doc, directories[Directories.OEBPS], $"{t.record.Id}.html"))
                )); 
        }

        private Dictionary<Directories, string> GetDirectoryDict(string rootDir, Guid folderId) => new Dictionary<Directories, string> {
            {Directories.ROOT, rootDir},
            {Directories.OEBPS, @$"{rootDir}\{folderId}\OEBPS" },
            {Directories.METAINF, @$"{rootDir}\{folderId}\META-INF" },
            {Directories.BOOKDIR,  @$"{rootDir}\{folderId}" },
            {Directories.IMAGES, @$"{rootDir}\{folderId}\OEBPS\image_repo" }
        };

        private async Task SaveTaskAsync(XDocument file, string toDirectory, string withFileName)
        {
            await using Stream stream = File.Create($"{toDirectory}/{withFileName}");
            await file.SaveAsync(stream, SaveOptions.None, CancellationToken.None);
        }

        private async Task SaveTaskAsync(HtmlDocument file, string toDirectory, string withFileName)
        {
            await using Stream stream = File.Create($"{toDirectory}/{withFileName}");
            await Task.Run(() => file.Save(stream));
        }

    }
}
