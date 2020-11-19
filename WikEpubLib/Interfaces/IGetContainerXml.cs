using System.Xml.Linq;

namespace WikEpubLib.Interfaces
{
    public interface IGetContainerXml
    {
        XDocument GetContainer();
    }
}