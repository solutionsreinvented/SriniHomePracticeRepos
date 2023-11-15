using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IReport
    {
        IProjectData ProjectData { get; }

        Document Document { get; }

        DataSource DataSource { get; }

        IReportContent Content { get; set; }
    }
}
