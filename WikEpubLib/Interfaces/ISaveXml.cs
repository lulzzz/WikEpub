using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WikEpubLib.Interfaces
{
    public interface ISaveXml
    {
        public Task To(string directory); 
    }
}
