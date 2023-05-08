using Newtonsoft.Json;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;

using SlvParkview.FinanceManager.Enums;
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

        private readonly Block _block;

        private readonly DateTime _reportTill;

        #endregion

        #region Default Constructor

        public BlockOutstandingsReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public BlockOutstandingsReport(Block block, DateTime reportTill, OutstandingsFilter filter = OutstandingsFilter.All, bool showPenaltiesOnly = false)
        {
            _block = block;
            _reportTill = reportTill;

            ShowPenaltiesOnly = showPenaltiesOnly;
            Filter = filter;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public List<FlatInfo> FlatInfoCollection { get => Get<List<FlatInfo>>(); private set => Set(value); }

        [JsonProperty]
        public string TotalOutstanding { get => Get<string>(); private set => Set(value); }

        [JsonProperty]
        public string TotalPenalty { get => Get<string>(); private set => Set(value); }

        public OutstandingsFilter Filter { get => Get<OutstandingsFilter>(); private set => Set(value); }

        public bool ShowPenaltiesOnly { get => Get<bool>(); private set => Set(value); }

        /// Exercise cuation while changing this string. This string is used to access the linked js files.
        private protected override string TemplateFileName => "Block Outstandings";

        #endregion

        #region Public Methods

        public override void GenerateContents()
        {
            FlatInfoCollection = new List<FlatInfo>();

            if (_block != null && _block.Flats != null)
            {
                foreach (Flat flat in _block.Flats)
                {
                    if (_block.ConsiderPenalties)
                    {
                        flat.GeneratePenalties(_block);
                    }
                    else
                    {
                        flat.Penalties.Clear();
                    }

                    flat.DateSpecified = _reportTill;

                    if (Filter == OutstandingsFilter.Defaulters)
                    {
                        if (flat.OutstandingOnSpecifiedDate >= _block.MinimumOutstandingForPenalty)
                        {
                            FlatInfoCollection.Add(flat.ParseToFlatInfo());
                        }
                    }
                    else
                    {
                        FlatInfoCollection.Add(flat.ParseToFlatInfo());
                    }
                }
            }

            TotalOutstanding = FlatInfoCollection?.Sum(f => decimal.Parse(StripNumberFormat(f.OutstandingOnSpecifiedDate))).FormatNumber("N1");
            TotalPenalty = FlatInfoCollection?.Sum(f => decimal.Parse(StripNumberFormat(f.PenaltyTillSpecifiedDate))).FormatNumber("N1");
        }

        #endregion

        #region Private Helper Methods

        private protected override void CreateRequiredDirectories()
        {
            base.CreateRequiredDirectories();

            /// Create the directory to store Block Outstandings Reports if it does not exists.
            _reportTargetDirectory = Path.Combine(FileServiceProvider.BlockOustandingsReportsDirectory);

            if (!Directory.Exists(_reportTargetDirectory))
            {
                _ = Directory.CreateDirectory(_reportTargetDirectory);
            }
        }

        private string StripNumberFormat(string value)
        {
            string unformattedString = value;

            if (value.Contains("(") && value.Contains(")"))
            {
                unformattedString = string.Concat("-", value.Replace("(", "").Replace(")", ""));
            }

            return unformattedString;
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
            if (ShowPenaltiesOnly)
            {
                return $"Summary of Penalties - As on {_reportTill:dd MMM yyyy}";
            }

            string penaltyString = _block.ConsiderPenalties ? "With Penalties" : "Without Penalties";
            ///return $"{_fileName} - As on {_reportTill:dd MMM yyyy} - {penaltyString} - {Filter}";
            return $"Oustanding Dues {penaltyString} As on {_reportTill:dd MMM yyyy} - {Filter}";
        }

        #endregion

    }
}
