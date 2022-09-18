namespace SlvParkview.FinanceManager.Reporting
{
    public interface IReport
    {
        string GeneratedOn { get; }

        string ApartmentName { get; }

        void Generate();
    }
}