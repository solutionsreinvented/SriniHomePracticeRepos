using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Reporting
{
    public class MonthlyPaymentsReport : Report, IReport
    {
        public MonthlyPaymentsReport()
        {

        }

        public string ReportedMonth { get => Get<string>(); set => Set(value); }

        public List<MonthlyPaymentRecord> Payments { get => Get<List<MonthlyPaymentRecord>>(); set => Set(value); }

        public override void Generate()
        {
            throw new System.NotImplementedException();
        }

        private protected override void CreateHtmlFile()
        {
            throw new System.NotImplementedException();
        }

        private protected override void CreateJsonFile()
        {
            throw new System.NotImplementedException();
        }

        private protected override void CreateRequiredDirectories()
        {
            throw new System.NotImplementedException();
        }
    }
}
