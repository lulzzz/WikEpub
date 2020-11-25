using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WikEpub.Models
{
    public class EpubFile
    {
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public IEnumerable<string> WikiPages { get; set; }
        public Guid guid { get; set; }

        
    }
}
