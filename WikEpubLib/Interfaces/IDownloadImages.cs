using System.Threading.Tasks;
using WikEpubLib.Records;

namespace WikEpubLib.Interfaces
{
    public interface IDownloadImages
    {
        public Task From(WikiPageRecord pageRecord, string oepbsDirectory);
    }
}