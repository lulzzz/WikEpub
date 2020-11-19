using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikEpubLib.Interfaces
{
    public interface IHtmlsToEpub
    {
        public Task GetEpub(IEnumerable<string> urls, string toRootDirectory, string asBookTitle, Guid guid);
    }
}