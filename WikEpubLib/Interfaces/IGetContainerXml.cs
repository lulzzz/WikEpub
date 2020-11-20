using System.Threading.Tasks;
using System.Xml.Linq;
using WikEpubLib.Enums;

namespace WikEpubLib.Interfaces
{
    public interface IGetContainerXml
    {
        Task<(XmlType, XDocument)> GetContainer();
    }
}