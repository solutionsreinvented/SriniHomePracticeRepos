using System.Collections.Generic;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class OverallSummary
    {
        public HashSet<OverallSummaryItem> Items { get; set; }
    }
}
