using System.Collections.Generic;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class OverallSummary
    {
        public HashSet<OverallSummaryTakeOffItem> Items { get; set; }
    }
}
