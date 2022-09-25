using Newtonsoft.Json;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Extensions;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlvParkview.FinanceManager.Reporting.Models
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

        private readonly PaymentModeFilter _paymentModeFilter;

        private readonly int _year;

        #endregion

        #region Default Constructor

        public MonthwisePaymentsReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public MonthwisePaymentsReport(Block block, Month forMonth, PaymentModeFilter paymentModeFilter, int year)
        {
            _block = block;
            _forMonth = forMonth;
            _paymentModeFilter = paymentModeFilter;
            _year = year;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public string ReportedMonth { get => Get<string>(); private set => Set(value); }

        [JsonProperty]
        public string ReportedYear { get => Get<string>(); private set => Set(value); }

        [JsonProperty]
        public List<PaymentInfo> Payments { get => Get<List<PaymentInfo>>(); private set => Set(value); }

        [JsonProperty]
        public string TotalPayment { get => Get<string>(); private set => Set(value); }

        #endregion

        #region Public Methods

        public override void GenerateContents()
        {
            ReportedMonth = _forMonth.ToString();
            ReportedYear = _year.ToString();

            List<PaymentInfo> allPayments = new List<PaymentInfo>();

            if (_block != null && _block.Flats != null)
            {
                foreach (Flat flat in _block.Flats)
                {
                    IEnumerable<Payment> flatPayments = flat.Payments?
                                                            .Where(p => p.ReceivedOn.Month == (int)_forMonth && p.ReceivedOn.Year == _year);

                    if (_paymentModeFilter == PaymentModeFilter.All)
                    {
                        flatPayments?.ToList()
                                     .ForEach(p => allPayments.Add(p.ParseToPaymentInfo(flat)));
                    }
                    else
                    {
                        flatPayments?.Where(p => p.Mode.ToString() == _paymentModeFilter.ToString())
                                     .ToList()
                                     .ForEach(p => allPayments.Add(p.ParseToPaymentInfo(flat)));
                    }
                }
            }

            Payments = allPayments?.OrderBy(p => DateTime.Parse(p.ReceivedOn)).ToList();
            TotalPayment = Payments.Sum(p => decimal.Parse(p.Amount)).FormatNumber("N1");
        }

        #endregion

        #region Private Helper Methods

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

        private protected override void CreateHtmlFile()
        {
            string fileName = $"{GetFileName()}.html";

            string[] htmlContents = File.ReadAllLines(Path.Combine(ServiceProvider.ReportTemplatesDirectory, $"{_fileName}.html"));

            List<string> finalHtmlFileContent = ConcatenateScriptTagIn(htmlContents, fileName);

            File.WriteAllLines(Path.Combine(_reportTargetDirectory, fileName), finalHtmlFileContent);
        }

        private protected override void CreateJavaScriptFile()
        {
            string fileName = $"{GetFileName()}.js";

            string jsFilePath = Path.Combine(ServiceProvider.ReportScriptsDirectory, $"{_fileName}.js");
            string[] jsContents = File.ReadAllLines(jsFilePath);

            string finalJavaScriptFileContent = ConcatenateJsonContentIn(jsContents);

            File.WriteAllText(Path.Combine(_reportTargetDirectory, fileName), finalJavaScriptFileContent);
        }

        #endregion

        #region Private Helper Functions

        private protected override string GetSerializedData()
        {
            IDataSerializer<MonthwisePaymentsReport> serializer = new JsonDataSerializer<MonthwisePaymentsReport>();

            return serializer.Serialize(this);
        }

        private protected override string GetFileName()
        {
            return $"{_fileName} ({_forMonth} {_year} - {_paymentModeFilter})";
        }

        #endregion

    }
}
