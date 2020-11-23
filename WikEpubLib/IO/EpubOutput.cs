using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Enums;
using WikEpubLib.Records;

namespace WikEpubLib.IO
{
    public class EpubOutput : IEpubOutput
    {
        private HttpClient _httpClient;

        public EpubOutput(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateDirectoriesAsync(Dictionary<Directories, string> directories) =>
            await Task.Run(() =>
            {
                Directory.CreateDirectory(directories[Directories.OEBPS]);
                Directory.CreateDirectory(directories[Directories.METAINF]);
                Directory.CreateDirectory(directories[Directories.IMAGES]);
            });

        public async Task CreateMimeFile(Dictionary<Directories, string> directories) =>
            await File.WriteAllTextAsync($@"{directories[Directories.BOOKDIR]}\mimetype", "application/epub+zip");

        public IEnumerable<Task> DownLoadImages(WikiPageRecord pageRecord, Dictionary<Directories, string> directories) =>
            pageRecord.SrcMap.AsParallel().WithDegreeOfParallelism(10).Select(async src =>
            {
                var srcKey = src.Key switch
                {
                    _ when src.Key.StartsWith("https://") => src.Key,
                    _ when src.Key.StartsWith(@"/api") => $"https://en.wikipedia.org{src.Key}",
                    _ when src.Key.StartsWith(@"//") => @$"https:{src.Key}",
                    _ => "unknown" 
                };
                if (srcKey == "unknown")
                {
                    Console.WriteLine($"Unknown image href encountered in {pageRecord.Id} wiki: \n" + src.Key);
                    return;
                }
                HttpResponseMessage response = await _httpClient.GetAsync(srcKey);
                using var memoryStream = await response.Content.ReadAsStreamAsync();
                await using var fileStream = File.Create($@"{directories[Directories.OEBPS]}\{src.Value}");
                await memoryStream.CopyToAsync(fileStream);
            });

        public async Task SaveDocumentsAsync(Dictionary<Directories, string> directories, IEnumerable<(XmlType type, XDocument doc)> xmlDocs) =>
            await Task.WhenAll(
                xmlDocs.Select(t => t.type switch
                {
                    XmlType.Container => SaveTaskAsync(t.doc, directories[Directories.METAINF], "container.xml"),
                    XmlType.Content => SaveTaskAsync(t.doc, directories[Directories.OEBPS], "content.opf"),
                    XmlType.Toc => SaveTaskAsync(t.doc, directories[Directories.OEBPS], "toc.ncx"),
                    _ => throw new ArgumentException("Unknown XML type found in xml switch expression")
                })
                );

        public async Task SaveDocumentsAsync(Dictionary<Directories, string> directories,
            IEnumerable<(HtmlDocument doc, WikiPageRecord record)> htmlDocs) =>
            await Task.WhenAll(htmlDocs.Select(t => SaveTaskAsync(t.doc, directories[Directories.OEBPS], $"{t.record.Id}.html")));

        public async Task ZipFiles(Dictionary<Directories, string> directories, Guid bookId) =>
            await Task.Run(() =>
            {
                ZipFile.CreateFromDirectory(
                    directories[Directories.BOOKDIR],
                    @$"{directories[Directories.BOOKDIR]}.epub");
                Directory.Delete(directories[Directories.BOOKDIR], true);
            });

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