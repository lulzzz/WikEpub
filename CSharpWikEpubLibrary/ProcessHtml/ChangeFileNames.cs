using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public class ChangeFileNames : IChangeFilesNames
    {
        private int _fileNumber;
        public Dictionary<string, string> MapOldToNewName { get; } = new Dictionary<string, string>();
        public void ChangeFileNamesIn(string directory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] info = directoryInfo.GetFiles();
            foreach (var fileInfo in info)
            {
                var oldFileNameWithType = fileInfo.Name;
                var oldFileNameWithoutType = oldFileNameWithType.Split('.').First();
                var newFileName = fileInfo.FullName.Replace(oldFileNameWithoutType, $"Image_{_fileNumber}");
                
                File.Move(fileInfo.FullName, newFileName );
                
                MapOldToNewName.Add(oldFileNameWithType, newFileName);
                _fileNumber++;
            } 
        }

    }
}
