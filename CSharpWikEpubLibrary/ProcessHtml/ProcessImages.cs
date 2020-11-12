using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using CSharpWikEpubLibrary.FileManager;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public class ProcessImages : IProcessImages
    {
        private readonly IDownloadFiles _downloadFiles;
        /// <summary>
        /// maps old file name to new file directory path
        /// </summary>
        private readonly Dictionary<string, string> _mapOldNameToNewDirPath = new Dictionary<string, string>();
        
        public ProcessImages(IDownloadFiles downloadFiles)
        {
            _downloadFiles = downloadFiles;
        }

        /// <summary>
        /// Downloads image files to specified directory, points image file path in html doc to new directory.
        /// If no image nodes are present in the document, an unaltered document will be returned
        /// </summary>
        /// <param name="inputDocument">Html document to transform</param>
        /// <param name="imageDirectory">Directory to save images to</param>
        /// <returns>Html document</returns>
        public HtmlDocument ProcessDownloadLinks(HtmlDocument inputDocument, string imageDirectory)
        {
            HtmlNode[] imageNodes = inputDocument
                    .DocumentNode
                    .Descendants()
                    .Where(node => node.Name == "img").ToArray();

            if (!imageNodes.Any())
                return inputDocument;

            // get the links to each image 
            string[] imageLinks = imageNodes.Select(node => node.GetAttributeValue("src", "no_value")).Distinct().ToArray();
            // Get Map of image links from html
            var imageLinkSet = imageLinks.ToHashSet();

            //download each link to a specified folder
            _downloadFiles.DownloadAsync(imageLinks.Select(link => $"https:{link}"), imageDirectory);
                        
            var title = inputDocument
                .DocumentNode
                .Descendants()
                .First(node => node.Name == "title")
                .InnerHtml
                .Split('-')
                .First()
                .Trim()
                .Replace(' ','_');

            ChangeFileNamesIn(imageDirectory, title);

            foreach (var node in imageNodes)
            {
                var srcValue = node.Attributes.First(a => a.Name == "src").Value;
                if (imageLinkSet.Contains(srcValue))
                    ChangeHtmlNodeAttribute(node, "src", _mapOldNameToNewDirPath[srcValue.Split('/').Last()]);
            }
            return inputDocument;
        }

        private void ChangeHtmlNodeAttribute(HtmlNode node, string attName, string newValue) =>
            node
                .Attributes
                .First(attribute => attribute.Name == attName)
                .Value = newValue;
        
        private int _fileNumber;
        private void ChangeFileNamesIn(string directory, string wikiTitle)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] info = directoryInfo.GetFiles();
            HashSet<string> directoryHashSet = info.Select(inf => inf.FullName).ToHashSet();

            foreach (var fileInfo in info)
            {
                var oldFileNameWithType = fileInfo.Name;
                var oldFileNameWithoutType = oldFileNameWithType.Split('.').First();
                var newFileName = fileInfo.FullName.Replace(oldFileNameWithoutType, $"{wikiTitle}_image_{_fileNumber}");

                if (directoryHashSet.Contains(newFileName))
                {
                    File.Delete(fileInfo.FullName);
                }
                else
                {
                    File.Move(fileInfo.FullName, newFileName );
                    _mapOldNameToNewDirPath.Add(oldFileNameWithType, newFileName);
                }
                _fileNumber++;
            } 
        }
    }
}
