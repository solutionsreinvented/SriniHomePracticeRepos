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
    public class ReceiptsInADateRangeReport : ReceiptsReport, IReport
    {
        #region Private Fields

        private const string _fileName = "Receipts History";

        private readonly DateTime _startDate;

        private readonly DateTime _endDate;

        #endregion

        #region Default Constructor

        public ReceiptsInADateRangeReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public ReceiptsInADateRangeReport(Block block, IReportOptions reportOptions)
            : base(block, reportOptions)
        {
            DateRangeReceiptsReportOptions dateRangeReportOptions = _reportOptions as DateRangeReceiptsReportOptions;

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

            List<ReceiptInfo> allReceipts = new List<ReceiptInfo>();

            if (_block != null && _block.Flats != null)
            {
                foreach (Flat flat in _block.Flats)
                {
                    IEnumerable<Receipt> flatReceipts = flat.Receipts?
                                                            .Where(p => p.ReceivedOn >= _startDate && p.ReceivedOn <= _endDate);

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
            IDataSerializer<ReceiptsInADateRangeReport> serializer = new JsonDataSerializer<ReceiptsInADateRangeReport>();

            return serializer.Serialize(this);
        }

        private protected override string GetFileName()
        {
            return $"{_fileName} ({StartDate} to {EndDate}) (Mode(s) - {_reportOptions.ReceiptModeFilter})";
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
