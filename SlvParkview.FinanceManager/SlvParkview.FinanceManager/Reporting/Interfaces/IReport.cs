namespace SlvParkview.FinanceManager.Reporting.Interfaces
{
    public interface IReport
    {
        string GeneratedOn { get; }

        string ApartmentName { get; }

        string DocumentTitle { get; }

        string HtmlFilePath { get; }

        void GenerateContents();

        void SaveReport();
    }
}