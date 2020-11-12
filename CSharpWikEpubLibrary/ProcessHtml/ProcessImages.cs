using System;
using System.Collections.Generic;
using System.IO;
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
        private Dictionary<string, string> _mapOldToNewName = new Dictionary<string, string>();
        
        public ProcessImages(IDownloadFiles downloadFiles)
        {
            _downloadFiles = downloadFiles;
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
            
            ChangeFileNamesIn(imageDirectory);


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
                        ChangeHtmlNodeAttribute(node, "src", _mapOldToNewName[srcValue.Split('/').Last()]);
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

        private int _fileNumber;
        private void ChangeFileNamesIn(string directory)
        {
            
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] info = directoryInfo.GetFiles();
            HashSet<string> directoryHashSet = info.Select(inf => inf.FullName).ToHashSet();

            foreach (var fileInfo in info)
            {
                var oldFileNameWithType = fileInfo.Name;
                var oldFileNameWithoutType = oldFileNameWithType.Split('.').First();
                var newFileName = fileInfo.FullName.Replace(oldFileNameWithoutType, $"Image_{_fileNumber}");

                if (directoryHashSet.Contains(newFileName))
                {
                    File.Delete(fileInfo.FullName);
                }
                else
                {
                    File.Move(fileInfo.FullName, newFileName );
                    _mapOldToNewName.Add(oldFileNameWithType, newFileName);
                }
                
                _fileNumber++;
            } 
        
            


        }



    }
}
