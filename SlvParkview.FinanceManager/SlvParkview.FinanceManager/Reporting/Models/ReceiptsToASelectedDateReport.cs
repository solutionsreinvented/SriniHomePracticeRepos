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
    /// Creates a report which comprises of the receipts received from each flat (of a given block) in a specified month.
    /// </summary>
    public class ReceiptsToASelectedDateReport : ReceiptsReport, IReport
    {
        #region Private Fields

        private const string _fileName = "Receipts History";

        private readonly DateTime _selectedDate;

        #endregion

        #region Default Constructor

        public ReceiptsToASelectedDateReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public ReceiptsToASelectedDateReport(Block block, IReportOptions reportOptions)
            : base(block, reportOptions)
        {
            ToASelectedDateReceiptsReportOptions toASelectedDateReportOptions = _reportOptions as ToASelectedDateReceiptsReportOptions;

            _selectedDate = toASelectedDateReportOptions.SelectedDate;
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

            List<ReceiptInfo> allReceipts = new List<ReceiptInfo>();

            if (_block != null && _block.Flats != null)
            {
                foreach (Flat flat in _block.Flats)
                {
                    IEnumerable<Receipt> flatReceipts = flat.Receipts?
                                                            .Where(p => p.ReceivedOn <= _selectedDate);

                    flatReceipts?.ToList().ForEach(p => allReceipts.Add(p.ParseToReceiptInfo(flat)));
                }
            }

            Receipts = ApplyFilterOn(allReceipts).OrderBy(p => DateTime.Parse(p.ReceivedOn)).ToList();
            TotalAmountReceived = Receipts.Sum(p => decimal.Parse(p.Amount)).FormatNumber("N1");
        }

        #endregion

        #region Private Helper Functions

        private protected override string GetSerializedData()
        {
            IDataSerializer<ReceiptsToASelectedDateReport> serializer = new JsonDataSerializer<ReceiptsToASelectedDateReport>();

            return serializer.Serialize(this);
        }

        private protected override string GetFileName()
        {
            return $"{_fileName} (As on {SelectedDate}) (Mode(s) - {_reportOptions.ReceiptModeFilter})";
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
