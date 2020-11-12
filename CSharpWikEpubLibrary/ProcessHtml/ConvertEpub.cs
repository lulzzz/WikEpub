using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CSharpWikEpubLibrary.ProcessHtml
{
    class ConvertEpub : IConvertEpub
    {
        private IParseHtmlDoc _getInitHtml;
        private IProcessImages _processImages;
        public ConvertEpub(IParseHtmlDoc getInitHtml, IProcessImages processImages)
        {
            _getInitHtml = getInitHtml;
            _processImages = processImages;
        }
        public void Convert(IEnumerable<string> urls)
        {
            throw new NotImplementedException();

        }
        
        private int _htmlId;
        private string HtmlId => $"doc_{_htmlId++}";
        private Dictionary<HtmlDocument,string> GetHtmlId(IEnumerable<HtmlDocument> htmlDocuments) =>
            htmlDocuments.ToDictionary( keySelector: document =>  document, document => HtmlId );
        

    }

}
