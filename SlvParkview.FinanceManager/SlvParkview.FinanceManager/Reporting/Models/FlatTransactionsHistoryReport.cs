using Newtonsoft.Json;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;

using SlvParkview.FinanceManager.Extensions;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;

using System;
using System.Collections.Generic;
using System.IO;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    /// <summary>
    /// Creates a report which contains the transactions (both payments and expenses) history for a specified flat
    /// calculated till the specified date.
    /// </summary>
    public class FlatTransactionsHistoryReport : Report
    {
        #region Private Fields

        private const string _fileName = "Transactions History";

        private readonly Flat _flat;

        private readonly DateTime _reportTill;

        #endregion

        #region Default Constructor

        public FlatTransactionsHistoryReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public FlatTransactionsHistoryReport(Flat flat, DateTime reportTill)
        {
            _flat = flat;
            _reportTill = reportTill;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public FlatInfo FlatInfo { get => Get<FlatInfo>(); private set => Set(value); }

        [JsonProperty]
        public List<TransactionInfo> Transactions { get => Get<List<TransactionInfo>>(); private set => Set(value); }

        #endregion

        #region Public Methods

        public override void GenerateContents()
        {
            if (_flat != null)
            {
                FlatInfo = _flat.ParseToFlatInfo();
                Transactions = _flat.GetTransactionsHistoryBasic(_reportTill);
            }
        }

        #endregion

        #region Private Helper Methods

        private protected override void CreateRequiredDirectories()
        {
            base.CreateRequiredDirectories();

            /// Create a new directory for Flatwise Reports if it does not exists.

            string flatwiseReportsDirectory = Path.Combine(ServiceProvider.FlatwiseReportsDirectory);

            if (!Directory.Exists(flatwiseReportsDirectory))
            {
                _ = Directory.CreateDirectory(flatwiseReportsDirectory);
            }

            /// Create if a separate directory for the selected flat does not exists.

            _reportTargetDirectory = Path.Combine(flatwiseReportsDirectory, _flat.Description);

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
            IDataSerializer<FlatTransactionsHistoryReport> serializer = new JsonDataSerializer<FlatTransactionsHistoryReport>();

            return serializer.Serialize(this);
        }

        private protected override string GetFileName()
        {
            return $"{_fileName} ({_reportTill:dd MMM yyyy})";
        }

        #endregion

    }
}
