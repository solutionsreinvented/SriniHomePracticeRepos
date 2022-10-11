using Newtonsoft.Json;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;

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
    public class PaymentsInADateRangeReport : PaymentsReport, IReport
    {
        #region Private Fields

        private const string _fileName = "Payments History";

        private readonly DateTime _startDate;

        private readonly DateTime _endDate;

        #endregion

        #region Default Constructor

        public PaymentsInADateRangeReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public PaymentsInADateRangeReport(Block block, IReportOptions reportOptions)
            : base(block, reportOptions)
        {
            DateRangePaymentsReportOptions dateRangeReportOptions = _reportOptions as DateRangePaymentsReportOptions;

            _startDate = dateRangeReportOptions.StartDate;
            _endDate = dateRangeReportOptions.EndDate;

        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public string StartDate { get => Get<string>(); private set => Set(value); }

        [JsonProperty]
        public string EndDate { get => Get<string>(); private set => Set(value); }

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
                                                            .Where(p => p.ReceivedOn >= _startDate && p.ReceivedOn <= _endDate);

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
            IDataSerializer<PaymentsInADateRangeReport> serializer = new JsonDataSerializer<PaymentsInADateRangeReport>();

            return serializer.Serialize(this);
        }

        private protected override string GetFileName()
        {
            return $"{_fileName} ({StartDate} to {EndDate}) (Mode(s) - {_reportOptions.PaymentModeFilter})";
        }

        #endregion

        #region Abstract Members Implementation

        private protected override void SetPrivateProperties()
        {
            StartDate = _startDate.ToString("dd-MMM-yyyy");
            EndDate = _endDate.ToString("dd-MMM-yyyy");
        }

        private protected override string GetTemplateFileName()
        {
            return _fileName;
        }

        #endregion

    }
}
