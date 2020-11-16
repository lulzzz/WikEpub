using System.Collections.Generic;

namespace WikEpubLib
{
    public record WikiPageRecord
    {
        string Id { get; init; }
        Dictionary<string, string> SrcMap { get; init; }
        IEnumerable<string> SectionHeadings { get; init; }
    }
}