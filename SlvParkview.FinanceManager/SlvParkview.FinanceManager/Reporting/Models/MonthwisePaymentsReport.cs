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
    public class MonthwisePaymentsReport : PaymentsReport, IReport
    {
        #region Private Fields

        private const string _fileName = "Payments History";

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
            : base(block)
        {
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
        public string Filter { get => Get<string>(); private set => Set(value); }

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

        #region Abstract Members Implementation

        private protected override void SetPrivateProperties()
        {
            ReportedMonth = _forMonth.ToString();
            ReportedYear = _year.ToString();
            Filter = _paymentModeFilter.ToString();
        }

        private protected override string GetTemplateFileName() => _fileName;

        #endregion

    }
}
