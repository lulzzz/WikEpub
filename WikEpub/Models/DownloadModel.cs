using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikEpub.Models
{
    public class DownloadModel
    {
        public string BookTitle { get; set; }
        public IEnumerable<string> WikiPages { get; set; }
        public Guid guid { get; set; }

        
    }
}
