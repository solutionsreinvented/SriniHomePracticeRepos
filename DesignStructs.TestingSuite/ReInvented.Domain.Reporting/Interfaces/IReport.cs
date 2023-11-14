using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IReport
    {
        IProjectInfo ProjectInfo { get; }

        Document Document { get; }

        DataSource DataSource { get; }

        IReportContent Content { get; set; }
    }
}
