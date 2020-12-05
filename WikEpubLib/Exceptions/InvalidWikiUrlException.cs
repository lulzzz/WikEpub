using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikEpubLib.Exceptions
{
    public class InvalidWikiUrlException : Exception
    {
        public InvalidWikiUrlException(IEnumerable<string> urls) : base($"Invalid urls: {urls}")
        { }
                
    }
}
