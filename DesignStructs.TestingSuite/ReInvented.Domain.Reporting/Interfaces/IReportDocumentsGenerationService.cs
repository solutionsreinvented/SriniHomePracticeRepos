using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IReportDocumentsGenerationService<T>
    {
        void Generate();
    }
}
