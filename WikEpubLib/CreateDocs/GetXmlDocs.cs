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

        public async Task<IEnumerable<(XmlType type, XDocument doc)>> FromAsync(IEnumerable<WikiPageRecord> pageRecords, string bookTitle) =>
            await Task.Run(() => new List<(XmlType type, XDocument doc)>()
            {
                ( XmlType.Container, _getContainerXml.GetContainer()),
                ( XmlType.Content, _getContentXml.From(pageRecords, bookTitle)),
                ( XmlType.Toc, _getTocXml.From(pageRecords, bookTitle) )
            });
    }
}