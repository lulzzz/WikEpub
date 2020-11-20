using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Enums;
using WikEpubLib.Interfaces;
using WikEpubLib.Records;

namespace WikEpubLib.CreateDocs
{
    public class GetXmlDocs : IGetXmlDocs
    {
        private readonly IGetTocXml _getTocXml;
        private readonly IGetContentXml _getContentXml;
        private readonly IGetContainerXml _getContainerXml;

        public GetXmlDocs(IGetTocXml getTocXml, IGetContentXml getContentXml, IGetContainerXml getContainerXml)
        {
            _getTocXml = getTocXml;
            _getContentXml = getContentXml;
            _getContainerXml = getContainerXml;
        }

        public IEnumerable<Task<(XmlType type, XDocument doc)>> From(IEnumerable<WikiPageRecord> pageRecords, string bookTitle) =>
            new List<Task<(XmlType type, XDocument doc)>>()
            {
                _getContainerXml.GetContainer(),
                _getContentXml.From(pageRecords, bookTitle),
                _getTocXml.From(pageRecords, bookTitle)
            };
    }
}