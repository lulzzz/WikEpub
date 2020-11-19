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
using WikEpubLib.Extensions;

namespace WikEpubLib.IO
{
    public class EpubOutput : IEpubOutput
    {
        private HttpClient _httpClient;

        public EpubOutput(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateDirectories(Dictionary<Directories, string> directories) =>
            await Task.Run(() =>
            {
                Directory.CreateDirectory(directories[Directories.OEBPS]);
                Directory.CreateDirectory(directories[Directories.METAINF]);
                Directory.CreateDirectory(directories[Directories.IMAGES]);
            });

        public async Task CreateMimeFile(Dictionary<Directories, string> directories) =>
            await File.WriteAllTextAsync($@"{directories[Directories.BOOKDIR]}\mimetype", "application/epub+zip");

        public async Task DownLoadImagesAsync(WikiPageRecord pageRecord, Dictionary<Directories, string> directories) =>
            await pageRecord.SrcMap.ToList().AsParallel().WithDegreeOfParallelism(10).ForEachAsync(async src =>
            {
                HttpResponseMessage responseResult = await _httpClient.GetAsync(@$"https:{src.Key}");
                using var memoryStream = await responseResult.Content.ReadAsStreamAsync();
                await using var fileStream = File.Create($@"{directories[Directories.OEBPS]}\{src.Value}");
                await memoryStream.CopyToAsync(fileStream);
            });

        public async Task SaveToAsync(Dictionary<Directories, string> directories, IEnumerable<(XmlType type, XDocument doc)> xmlDocs,
            IEnumerable<(HtmlDocument doc, WikiPageRecord record)> htmlDocs) =>
            await Task.WhenAll(
                xmlDocs.Select(t => t.type switch
                {
                    XmlType.Container => SaveTaskAsync(t.doc, directories[Directories.METAINF], "container.xml"),
                    XmlType.Content => SaveTaskAsync(t.doc, directories[Directories.OEBPS], "content.opf"),
                    XmlType.Toc => SaveTaskAsync(t.doc, directories[Directories.OEBPS], "toc.ncx"),
                    _ => throw new ArgumentException("Unknown XML type found in xml switch expression")
                }).Concat(htmlDocs.Select(t => SaveTaskAsync(t.doc, directories[Directories.OEBPS], $"{t.record.Id}.html"))
                ));

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