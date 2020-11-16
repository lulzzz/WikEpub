using System.Collections.Generic;

namespace WikEpubLib
{
    public record WikiPageRecord
    {
        public string Id;
        public Dictionary<string, string> SrcMap;
        public IEnumerable<(string id, string sectionName)> SectionHeadings;
    }

}