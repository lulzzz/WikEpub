using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ScrapeWiki
{
    class ProcessPictureDownloadLinks : IProcessPictureDownloadLinks
    {
        public HtmlDocument ProcessDownloadLinks(HtmlDocument inputDocument)
        {
            throw new NotImplementedException();
        }

        private List<string> GetDownLoadLinks(HtmlDocument inputDocument)
        {
            throw new NotImplementedException();

        }

        private HtmlDocument ChangeLocalFileReferences(HtmlDocument inputHtmlDocument, Dictionary<string, string> oldNewFileMap)
        {  
            throw new NotImplementedException();
        }
    }
}
