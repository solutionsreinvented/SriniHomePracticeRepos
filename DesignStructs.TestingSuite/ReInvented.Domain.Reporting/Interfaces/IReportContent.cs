using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IReportContent
    {
        DataSourceInformation DataSourceInformation { get; set; }
    }
}
