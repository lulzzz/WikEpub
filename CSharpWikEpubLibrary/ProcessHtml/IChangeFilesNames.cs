using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    public interface IChangeFilesNames
    {
        void ChangeFileNamesIn(string directory);

        Dictionary<string, string> MapOldToNewName { get; }
    }
}