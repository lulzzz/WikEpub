using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WikEpubLib.Exceptions
{
    public class InvalidWikiUrlException : Exception
    {
        static Regex regex = new Regex("(https:\\/\\/)?(en\\.)?wikipedia\\.org\\/(wiki\\/\\b(([-a-zA-Z0-9()@:%_\\+.~#?&\\/\\/=,]*){1}))");
        public InvalidWikiUrlException(IEnumerable<string> urls) : 
            base($"Invalid urls: {urls.Select(x => regex.IsMatch(x) ? x : $"-->{x}<--").Aggregate((x, y) =>  x + "\n"+ y)}")
        { }
                
    }
}
