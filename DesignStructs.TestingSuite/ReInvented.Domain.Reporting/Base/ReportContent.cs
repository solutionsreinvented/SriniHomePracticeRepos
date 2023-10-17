using System;

using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Base
{
    public class ReportContent : IReportContent
    {
        public ReportContent()
        {
            GeneratedOn = DateTime.Now;
        }

        public DateTime GeneratedOn { get; private set; }

        public DataSourceInformation DataSourceInformation { get; private set; }
    }
}
