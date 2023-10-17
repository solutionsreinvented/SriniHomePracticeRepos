using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Interfaces;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class MaterialTakeOff : ReportContent, IReportContent
    {
        public OverallSummary OverallSummary { get; set; }

        public PropertyWiseSummary PropertyWiseSummary { get; set; }

        public Contingencies Contingencies { get; set; }
    }
}
