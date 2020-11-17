using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikEpubLib.Interfaces;

namespace WikEpubLib
{
    public class SaveXml : ISaveXml
    {
        private readonly IGetTocXml toc;
        private readonly IGetContainerXml container;
        private readonly IGetContentXml content;

        public SaveXml(IGetTocXml toc, IGetContainerXml container, IGetContentXml content)
        {
            this.toc = toc;
            this.container = container;
            this.content = content;
        }
        public Task To(string directory)
        {
            throw new NotImplementedException();
        }
    }
}
