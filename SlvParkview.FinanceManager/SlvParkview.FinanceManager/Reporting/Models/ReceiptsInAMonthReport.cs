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
    /// Creates a report which comprises of the receipts received from each flat (of a given block) in a specified month.
    /// </summary>
    public class ReceiptsInAMonthReport : ReceiptsReport, IReport
    {
        #region Private Fields

        private const string _fileName = "Receipts History";

        private readonly Month _forMonth;

        private readonly int _year;

        #endregion

        #region Default Constructor

        public ReceiptsInAMonthReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public ReceiptsInAMonthReport(Block block, IReportOptions reportOptions)
            : base(block, reportOptions)
        {
            InAMonthReceiptsReportOptions inAMonthReportOptions = _reportOptions as InAMonthReceiptsReportOptions;

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

            List<ReceiptInfo> allReceipts = new List<ReceiptInfo>();

            if (_block != null && _block.Flats != null)
            {
                foreach (Flat flat in _block.Flats)
                {
                    IEnumerable<Receipt> flatReceipts = flat.Receipts?
                                                            .Where(p => p.ReceivedOn.Month == (int)_forMonth && p.ReceivedOn.Year == _year);

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
            IDataSerializer<ReceiptsInAMonthReport> serializer = new JsonDataSerializer<ReceiptsInAMonthReport>();

            return serializer.Serialize(this);
        }

        private protected override string GetFileName()
        {
            return $"{_fileName} ({_forMonth} {_year} - {_reportOptions.ReceiptModeFilter})";
        }

        #endregion

        #region Abstract Members Implementation

        private protected override void SetPrivateProperties()
        {
            ReportedMonth = _forMonth.ToString();
            ReportedYear = _year.ToString();
            ///Filter = _reportOptions.ReceiptModeFilter.ToString();
        }

        private protected override string GetTemplateFileName() => _fileName;

        #endregion

    }
}
