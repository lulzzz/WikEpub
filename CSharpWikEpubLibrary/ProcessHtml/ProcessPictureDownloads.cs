using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public class ProcessPictureDownloads : IProcessPictureDownloads
    {
        private IDownloadFiles _downloadFiles;

        public ProcessPictureDownloads(IDownloadFiles downloadFiles)
        {
            _downloadFiles = downloadFiles;
        }

        public HtmlDocument ProcessDownloadLinks(HtmlDocument inputDocument)
        {
            GetDownLoadLinks(inputDocument).ToList().ForEach(Console.WriteLine);
            throw new NotImplementedException();
        }

        private IEnumerable<string> GetDownLoadLinks(HtmlDocument inputDocument) =>
            inputDocument.DocumentNode
                .Descendants()
                .Where(node => node.Name == "img")
                .Select(node => node.GetAttributeValue("src", "no_value"));



        private HtmlDocument ChangeLocalFileReferences(HtmlDocument inputHtmlDocument, Dictionary<string, string> oldNewFileMap)
        {  
            throw new NotImplementedException();
        }
    }
}
