namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IReportContentGenerationService
    {
        T Generate<T>() where T : IReportContent;
    }
}
