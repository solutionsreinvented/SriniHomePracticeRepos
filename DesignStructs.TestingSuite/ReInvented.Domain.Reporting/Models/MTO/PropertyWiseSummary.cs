using System.Collections.Generic;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class PropertyWiseSummary
    {
        public HashSet<PropertyWiseSummaryItem> Items { get; set; }
    }
}
