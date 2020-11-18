using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikEpubLib.Interfaces
{
    public interface IHtmlsToEpub
    {
        public Task Transform(IEnumerable<string> urls, string toRootDirectory, string asBookTitle, Guid guid);
    }
}
