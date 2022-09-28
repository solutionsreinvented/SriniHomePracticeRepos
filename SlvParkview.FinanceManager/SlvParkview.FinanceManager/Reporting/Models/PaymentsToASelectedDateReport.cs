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
    public class PaymentsToASelectedDateReport : PaymentsReport, IReport
    {
        #region Private Fields

        private const string _fileName = "Payments History";

        private readonly DateTime _selectedDate;

        #endregion

        #region Default Constructor

        public PaymentsToASelectedDateReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public PaymentsToASelectedDateReport(Block block, DateTime selectedDate) : base(block)
        {
            _selectedDate = selectedDate;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public string SelectedDate { get => Get<string>(); private set => Set(value); }

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
                                                            .Where(p => p.ReceivedOn <= _selectedDate);

                    flatPayments?.ToList().ForEach(p => allPayments.Add(p.ParseToPaymentInfo(flat)));
                }
            }

            Payments = allPayments?.OrderBy(p => DateTime.Parse(p.ReceivedOn)).ToList();
            TotalPayment = Payments.Sum(p => decimal.Parse(p.Amount)).FormatNumber("N1");
        }

        #endregion

        #region Private Helper Functions

        private protected override string GetSerializedData()
        {
            IDataSerializer<PaymentsToASelectedDateReport> serializer = new JsonDataSerializer<PaymentsToASelectedDateReport>();

            return serializer.Serialize(this);
        }

        private protected override string GetFileName()
        {
            return $"{_fileName} Upto {SelectedDate}";
        }

        #endregion

        #region Abstract Members Implementation

        private protected override void SetPrivateProperties()
        {
            SelectedDate = _selectedDate.ToString("dd-MMM-yyyy");
        }

        private protected override string GetTemplateFileName()
        {
            return _fileName;
        }

        #endregion

    }
}
