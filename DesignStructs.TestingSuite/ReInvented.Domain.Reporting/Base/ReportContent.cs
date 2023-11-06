using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Base
{
    public class ReportContent : IReportContent
    {
        public ReportContent()
        {

        }

        public DataSourceInformation DataSourceInformation { get; set; }
    }
}
