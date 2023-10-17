using System.Collections.Generic;

using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Interfaces;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class FoundationLoadData : ReportContent, IReportContent
    {
        public FoundationLoadData()
        {

        }

        public Dictionary<int, string> LoadCases { get; set; }

    }

}
