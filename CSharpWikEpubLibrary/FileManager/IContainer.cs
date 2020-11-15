using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CSharpWikEpubLibrary.FileManager
{
    public interface IContainer
    {
        Task CreateAsync(string rootDirectory);
    }
}