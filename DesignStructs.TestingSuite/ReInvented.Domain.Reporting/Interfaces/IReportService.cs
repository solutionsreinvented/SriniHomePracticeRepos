namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IReportService
    {
        T Generate<T>() where T : IReport;
    }
}
