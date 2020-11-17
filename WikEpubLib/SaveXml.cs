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
        private readonly IToc toc;
        private readonly IContainer container;
        private readonly IContent content;

        public SaveXml(IToc toc, IContainer container, IContent content)
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
