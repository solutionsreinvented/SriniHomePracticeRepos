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
    }
}
