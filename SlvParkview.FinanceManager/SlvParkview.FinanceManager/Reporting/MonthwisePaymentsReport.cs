using Newtonsoft.Json;
using SlvParkview.FinanceManager.Models;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Reporting
{
    public class MonthwisePaymentsReport : Report, IReport
    {
        #region Private Fields

        private readonly Block _block;
        private readonly int _monthOfTheYear;

        #endregion

        #region Parameterized Constructor

        public MonthwisePaymentsReport(Block block, int monthOfTheYear)
        {
            _block = block;
            _monthOfTheYear = monthOfTheYear;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public string ReportedMonth { get => Get<string>(); private set => Set(value); }

        [JsonProperty]
        public List<MonthlyPaymentRecord> Payments { get => Get<List<MonthlyPaymentRecord>>(); private set => Set(value); } 

        #endregion

        private protected override string GetSerializedData()
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
            base.CreateRequiredDirectories();

            throw new System.NotImplementedException();
        }
    }
}
