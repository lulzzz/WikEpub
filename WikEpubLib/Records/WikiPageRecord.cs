using System.Collections.Generic;

namespace WikEpubLib.Records
{
    public record WikiPageRecord
    {
        public string Id;
        public Dictionary<string, string> SrcMap;
        public List<(string id, string sectionName)> SectionHeadings;
    }
}