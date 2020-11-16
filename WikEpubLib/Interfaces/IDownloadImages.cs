using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WikEpubLib.Interfaces
{
    public interface IDownloadImages
    {
        public Task From(WikiPageRecord pageRecord, string oepbsDirectory, HttpClient httpClient);
    }
}
