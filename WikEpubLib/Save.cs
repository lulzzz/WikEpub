using HtmlAgilityPack;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WikEpubLib
{
    public static class Save
    {
        public static async Task WithAsyncTask(XDocument file, string toDirectory, string withFileName)
        {
            await using Stream stream = File.Create($"{toDirectory}/{withFileName}");
            await file.SaveAsync(stream, SaveOptions.None, CancellationToken.None);
        }

        public static async Task WithAsyncTask(HtmlDocument file, string toDirectory, string withFileName)
        {
            await using Stream stream = File.Create($"{toDirectory}/{withFileName}");
            await Task.Run(() => file.Save(stream));
        }
    }
}