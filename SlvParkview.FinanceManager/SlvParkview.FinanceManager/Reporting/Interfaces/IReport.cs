namespace SlvParkview.FinanceManager.Reporting.Interfaces
{
    public interface IReport
    {
        string GeneratedOn { get; }

        string ApartmentName { get; }

        void GenerateContents();

        void SaveReport();
    }
}