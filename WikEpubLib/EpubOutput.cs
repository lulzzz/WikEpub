using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WikEpubLib
{
    public class EpubOutput : IEpubOutput
    {
        HttpClient _httpClient;

        public EpubOutput(HttpClient httpClient)
        {
           _httpClient = httpClient;
        }

        public async Task CreateDirectories(string rootDirectory, Guid folderID) =>
            await Task.Run(() =>
            {
                Directory.CreateDirectory(@$"{rootDirectory}\{folderID}\OEBPS");
                Directory.CreateDirectory(@$"{rootDirectory}\{folderID}\META-INF");
                Directory.CreateDirectory(@$"{rootDirectory}\{folderID}\OEBPS\image_repo");
            });

        public async Task DownLoadImagesAsync(WikiPageRecord pageRecord, Dictionary<Directories, string> directories) =>
            await pageRecord.SrcMap.ToList().ForEachAsync(async src =>
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
