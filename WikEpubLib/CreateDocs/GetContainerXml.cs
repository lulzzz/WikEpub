using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Interfaces;

namespace WikEpubLib.CreateDocs
{
    public class GetContainerXml : IGetContainerXml
    {
        public XDocument GetContainer()
        {
            XNamespace defaultNs = "urn:oasis:names:tc:opendocument:xmlns:container";
            return new XDocument(
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
            );
        }
    }
}
