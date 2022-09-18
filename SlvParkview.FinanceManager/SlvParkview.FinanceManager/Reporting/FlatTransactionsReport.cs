﻿using Newtonsoft.Json;
using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace SlvParkview.FinanceManager.Reporting
{
    public class FlatTransactionsReport : Report
    {
        #region Private Fields

        private const string _fileName = "Transactions History";

        private readonly Flat _flat;

        private readonly DateTime _reportTill;

        #endregion

        #region Default Constructor

        public FlatTransactionsReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public FlatTransactionsReport(Flat flat, DateTime reportTill)
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

        #region Private Helper Methods

        private protected override void CreateRequiredDirectories()
        {
            base.CreateRequiredDirectories();

            string flatwiseReportsDirectory = Path.Combine(ServiceProvider.FlatWiseReportsDirectory);

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
            IDataSerializer<FlatTransactionsReport> serializer = new JsonDataSerializer<FlatTransactionsReport>();

            return serializer.Serialize(this);
        }

        #endregion

    }
}
