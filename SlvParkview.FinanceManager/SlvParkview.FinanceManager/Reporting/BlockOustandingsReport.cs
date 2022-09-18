using Newtonsoft.Json;
using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace SlvParkview.FinanceManager.Reporting
{
    public class BlockOustandingsReport : Report, IReport
    {
        #region Private Fields

        private const string _fileName = "Block Outstandings";

        private readonly Block _block;

        private readonly DateTime _reportTill;

        #endregion

        #region Default Constructor

        public BlockOustandingsReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public BlockOustandingsReport(Block block, DateTime reportTill)
        {
            _block = block;
            _reportTill = reportTill;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public List<FlatInfo> FlatInfoCollection { get => Get<List<FlatInfo>>(); private set => Set(value); }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Creates the required directories to store the json and html files of the report(s).
        /// </summary>
        private protected override void CreateRequiredDirectories()
        {
            base.CreateRequiredDirectories();

            /// Create if a separate directory for the selected flat does not exists.

            _reportTargetDirectory = Path.Combine(ServiceProvider.BlockOustandingsReportsDirectory);

            if (!Directory.Exists(_reportTargetDirectory))
            {
                _ = Directory.CreateDirectory(_reportTargetDirectory);
            }
        }

        private protected override void CreateHtmlFile()
        {
            string fileName = $"{_fileName}({_reportTill:dd MMM yyyy}).html";

            File.Copy(Path.Combine(ServiceProvider.ReportTemplatesDirectory, $"{_fileName}.html"),
                                   Path.Combine(_reportTargetDirectory, fileName), true);
        }

        private protected override void CreateJsonFile()
        {
            string fileName = $"{_fileName}({_reportTill:dd MMM yyyy}).json";

            File.WriteAllText(Path.Combine(_reportTargetDirectory, fileName), GetSerializedData());
        }

        #endregion

        #region Private Helper Functions

        private protected override string GetSerializedData()
        {
            IDataSerializer<BlockOustandingsReport> serializer = new JsonDataSerializer<BlockOustandingsReport>();

            return serializer.Serialize(this);
        }

        #endregion

    }
}
