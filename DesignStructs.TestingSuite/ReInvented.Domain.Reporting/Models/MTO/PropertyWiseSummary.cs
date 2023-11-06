using System.Collections.Generic;
using System.Linq;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class PropertyWiseSummary
    {
        public HashSet<PropertyWiseSummaryItem> Items { get; set; }

        public double Total => Items == null ? 0.0 : Items.Sum(i => i.Weight);
    }
}
