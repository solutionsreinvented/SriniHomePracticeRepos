using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IReport<T>
    {
        DocumentInfo DocumentInfo { get; }

        DataSourceInformation DataSourceInformation { get; }

        T Content { get; set; }
    }
}
