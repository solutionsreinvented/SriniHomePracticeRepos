using Newtonsoft.Json;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;

using SlvParkview.FinanceManager.Extensions;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Models.Base;
using SlvParkview.FinanceManager.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    /// <summary>
    /// Creates a report that consists of flat wise outstanding (for a given block) calculated as on the specified date.
    /// </summary>
    public class BlockOutstandingsReport : Report, IReport
    {
        #region Private Fields

        private const string _fileName = "Block Outstandings";

        private readonly Block _block;

        private readonly DateTime _reportTill;

        #endregion

        #region Default Constructor

        public BlockOutstandingsReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public BlockOutstandingsReport(Block block, DateTime reportTill)
        {
            _block = block;
            _reportTill = reportTill;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public List<FlatInfo> FlatInfoCollection { get => Get<List<FlatInfo>>(); private set => Set(value); }

        [JsonProperty]
        public string TotalOutstanding { get => Get<string>(); private set => Set(value); }

        #endregion

        #region Public Methods

        public override void GenerateContents()
        {
            FlatInfoCollection = new List<FlatInfo>();

            if (_block != null && _block.Flats != null)
            {
                foreach (Flat flat in _block.Flats)
                {
                    flat.DateSpecified = _reportTill;
                    FlatInfoCollection.Add(flat.ParseToFlatInfo());
                }
            }

            TotalOutstanding = FlatInfoCollection?.Sum(f => decimal.Parse(StripNumberFormat(f.OutstandingOnSpecifiedDate))).FormatNumber("N1");
        }

        #endregion

        private string StripNumberFormat(string value)
        {
            string unformattedString = value;

            if (value.Contains("(") && value.Contains(")"))
            {
                unformattedString = string.Concat("-", value.Replace("(", "").Replace(")", ""));
            }

            return unformattedString;
        }


        #region Private Helper Methods

        private protected override void CreateRequiredDirectories()
        {
            base.CreateRequiredDirectories();

            /// Create the directory to store Block Outstandings Reports if it does not exists.
            _reportTargetDirectory = Path.Combine(ServiceProvider.BlockOustandingsReportsDirectory);

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
            IDataSerializer<BlockOutstandingsReport> serializer = new JsonDataSerializer<BlockOutstandingsReport>();

            return serializer.Serialize(this);
        }

        private protected override string GetFileName()
        {
            return $"{_fileName} ({_reportTill:dd MMM yyyy})";
        }

        #endregion

    }
}
