using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IReport<T>
    {
        IProjectInfo ProjectInfo { get; }

        Document Document { get; }

        DataSource DataSource { get; }

        T Content { get; set; }
    }
}
