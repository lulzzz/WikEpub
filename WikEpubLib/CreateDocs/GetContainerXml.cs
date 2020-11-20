using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Enums;
using WikEpubLib.Interfaces;

namespace WikEpubLib.CreateDocs
{
    public class GetContainerXml : IGetContainerXml
    {
        public async Task<(XmlType, XDocument)> GetContainer()
        {
            XNamespace defaultNs = "urn:oasis:names:tc:opendocument:xmlns:container";
            return await Task.Run(() => (XmlType.Container, new XDocument(
              new XElement(
                  defaultNs + "container",
                  new XElement(
                      defaultNs + "rootfiles",
                      new XElement(
                          defaultNs + "rootfile",
                          new XAttribute(
                              "full-path", "OEBPS/content.opf"
                          ),
                          new XAttribute(
                              "media-type", "application/oebps-package+xml"
                          )
                      )
                  )
              )
          )));
        }
    }
}