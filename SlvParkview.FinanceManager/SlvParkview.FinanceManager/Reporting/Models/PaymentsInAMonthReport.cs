using Newtonsoft.Json;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Extensions;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    /// <summary>
    /// Creates a report which comprises of the payments received from each flat (of a given block) in a specified month.
    /// </summary>
    public class PaymentsInAMonthReport : PaymentsReport, IReport
    {
        #region Private Fields

        private const string _fileName = "Payments History";

        private readonly Month _forMonth;

        private readonly int _year;

        #endregion

        #region Default Constructor

        public PaymentsInAMonthReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public PaymentsInAMonthReport(Block block, IReportOptions reportOptions)
            : base(block, reportOptions)
        {
            InAMonthPaymentsReportOptions inAMonthReportOptions = _reportOptions as InAMonthPaymentsReportOptions;

            _forMonth = inAMonthReportOptions.SelectedMonth;
            _year = inAMonthReportOptions.SelectedYear;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public string ReportedMonth { get => Get<string>(); private set => Set(value); }

        [JsonProperty]
        public string ReportedYear { get => Get<string>(); private set => Set(value); }

        //[JsonProperty]
        //public string Filter { get => Get<string>(); private set => Set(value); }

        #endregion

        #region Public Methods

        public override void GenerateContents()
        {
            SetPrivateProperties();

            List<PaymentInfo> allPayments = new List<PaymentInfo>();

            if (_block != null && _block.Flats != null)
            {
                foreach (Flat flat in _block.Flats)
                {
                    IEnumerable<Payment> flatPayments = flat.Payments?
                                                            .Where(p => p.ReceivedOn.Month == (int)_forMonth && p.ReceivedOn.Year == _year);

                    flatPayments?.ToList().ForEach(p => allPayments.Add(p.ParseToPaymentInfo(flat)));
                }
            }

            Payments = ApplyFilterOn(allPayments).OrderBy(p => DateTime.Parse(p.ReceivedOn)).ToList();
            TotalPayment = Payments.Sum(p => decimal.Parse(p.Amount)).FormatNumber("N1");
        }

        #endregion

        #region Private Helper Functions

        private protected override string GetSerializedData()
        {
            IDataSerializer<PaymentsInAMonthReport> serializer = new JsonDataSerializer<PaymentsInAMonthReport>();

            return serializer.Serialize(this);
        }

        private protected override string GetFileName()
        {
            return $"{_fileName} ({_forMonth} {_year} - {_reportOptions.PaymentModeFilter})";
        }

        #endregion

        #region Abstract Members Implementation

        private protected override void SetPrivateProperties()
        {
            ReportedMonth = _forMonth.ToString();
            ReportedYear = _year.ToString();
            ///Filter = _reportOptions.PaymentModeFilter.ToString();
        }

        private protected override string GetTemplateFileName() => _fileName;

        #endregion

    }
}
