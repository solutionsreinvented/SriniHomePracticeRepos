using Newtonsoft.Json;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Extensions;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlvParkview.FinanceManager.Reporting
{
    /// <summary>
    /// Creates a report which comprises of the payments received from each flat (of a given block) in a specified month.
    /// </summary>
    public class MonthwisePaymentsReport : Report, IReport
    {
        #region Private Fields

        private const string _fileName = "Monthwise Payments";

        private readonly Block _block;

        private readonly Month _forMonth;

        private readonly int _year;

        #endregion

        #region Default Constructor

        public MonthwisePaymentsReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public MonthwisePaymentsReport(Block block, Month forMonth, int year)
        {
            _block = block;
            _forMonth = forMonth;
            _year = year;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public string ReportedMonth { get => Get<string>(); private set => Set(value); }

        [JsonProperty]
        public List<PaymentInfo> Payments { get => Get<List<PaymentInfo>>(); private set => Set(value); }

        #endregion

        #region Private Helper Methods

        private protected override void GenerateContents()
        {
            ReportedMonth = _forMonth.ToString();
            Payments = new List<PaymentInfo>();

            if (_block != null && _block.Flats != null)
            {
                foreach (Flat flat in _block.Flats)
                {
                    IEnumerable<Payment> flatPayments = flat.Payments
                                                            .Where(p => p.ReceivedOn.Month == (int)_forMonth && p.ReceivedOn.Year == _year);
                    flatPayments?.ToList().ForEach(p => Payments.Add(p.ParseToPaymentInfo(flat.Description)));
                }
            }
        }

        private protected override void CreateHtmlFile()
        {
            string fileName = $"{_fileName} ({_forMonth}{_year}).html";

            File.Copy(Path.Combine(ServiceProvider.ReportTemplatesDirectory, $"{_fileName}.html"),
                                   Path.Combine(_reportTargetDirectory, fileName), true);
        }

        private protected override void CreateJsonFile()
        {
            string fileName = $"{_fileName} ({_forMonth}{_year}).json";

            File.WriteAllText(Path.Combine(_reportTargetDirectory, fileName), GetSerializedData());
        }

        private protected override void CreateRequiredDirectories()
        {
            base.CreateRequiredDirectories();

            /// Create a new directory to store Monthwise Payments reports if it does not exists.

            _reportTargetDirectory = Path.Combine(ServiceProvider.MonthwisePaymentsDirectory);

            if (!Directory.Exists(_reportTargetDirectory))
            {
                _ = Directory.CreateDirectory(_reportTargetDirectory);
            }
        }

        #endregion

        #region Private Helper Functions

        private protected override string GetSerializedData()
        {
            IDataSerializer<MonthwisePaymentsReport> serializer = new JsonDataSerializer<MonthwisePaymentsReport>();

            return serializer.Serialize(this);
        }

        #endregion

    }
}
