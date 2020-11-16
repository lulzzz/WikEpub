using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikEpubLib.Interfaces
{
    public interface IHtmlsToEpub
    {
        public Task Transform(IEnumerable<string> withUrls, string toRootDirectory, string asBookTitle);
    }
}
