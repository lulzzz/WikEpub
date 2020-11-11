using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using CSharpWikEpubLibrary.ProcessHtml;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ScrapeWiki
{
    public class ProcessImages : IProcessImages
    {
        private readonly IDownloadFiles _downloadFiles;
        private readonly IChangeFilesNames _changeFilesNames;
        
        //TODO incorp change Filenames into class
        public ProcessImages(IDownloadFiles downloadFiles, IChangeFilesNames changeFileNames )
        {
            _downloadFiles = downloadFiles;
            _changeFilesNames = changeFileNames;
        }

        public HtmlDocument ProcessDownloadLinks(HtmlDocument inputDocument, string imageDirectory)
        {
            IEnumerable<string> imageLinks;
            try
            {
                 // get the links to each image 
                imageLinks = GetImageLinks(inputDocument).Distinct();
            }
            catch (ArgumentNullException)
            {
                return inputDocument;
            }
            
                       
            //download each link to a specified folder
            _downloadFiles.DownloadAsync(imageLinks.Select(link => $"https:{link}"), imageDirectory);

            // Get Map of image links from html
            var imageLinkSet = imageLinks.ToHashSet();
            
            _changeFilesNames.ChangeFileNamesIn(imageDirectory);

            var oldNewFileNameMap = _changeFilesNames.MapOldToNewName;

            inputDocument
                .DocumentNode
                .Descendants()
                .Where(node => node.Name == "img")
                .ToList()
                .ForEach(node =>
                {
                    var srcValue = node.Attributes.First(a => a.Name == "src").Value;
                    if (imageLinkSet.Contains(srcValue))
                    {
                        ChangeHtmlNodeAttribute(node, "src", oldNewFileNameMap[srcValue.Split('/').Last()]);
                    }
                });
            
            return inputDocument;
        }

        private void ChangeHtmlNodeAttribute(HtmlNode node, string attName, string newValue) =>
            node
                .Attributes
                .First(attribute => attribute.Name == attName)
                .Value = newValue;
        

        private IEnumerable<string> GetImageLinks(HtmlDocument inputDocument) =>
            inputDocument.DocumentNode
                .Descendants()
                .Where(node => node.Name == "img")
                .Select(node => node.GetAttributeValue("src", "no_value"));



    }
}
