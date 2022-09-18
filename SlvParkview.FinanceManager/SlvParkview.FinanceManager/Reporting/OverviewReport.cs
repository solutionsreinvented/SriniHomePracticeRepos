using SlvParkview.FinanceManager.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace SlvParkview.FinanceManager.Reporting
{
    public class OverviewReport : Report, IReport
    {
        public List<FlatInfo> Flats { get => Get<List<FlatInfo>>(); set => Set(value); }

        public override void Generate()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates the required directories to store the json and html files of the report(s).
        /// </summary>
        private protected override void CreateRequiredDirectories()
        {
            /// Create if the Reports directory does not exists.
            if (!Directory.Exists(ServiceProvider.ReportsDirectory))
            {
                _ = Directory.CreateDirectory(ServiceProvider.ReportsDirectory);
            }

            /// Create if a separate directory for the selected flat does not exists.

            string flatDirectory = Path.Combine(ServiceProvider.ReportsDirectory, FlatToBeProcessed.Description);

            if (!Directory.Exists(flatDirectory))
            {
                _ = Directory.CreateDirectory(flatDirectory);
            }

            /// Create a directory with requested date if it does not exists.

            string todayDirectory = Path.Combine(flatDirectory, ReportTill.ToString("dd MMM yyyy"));

            if (!Directory.Exists(todayDirectory))
            {
                _ = Directory.CreateDirectory(todayDirectory);
            }
            _reportTargetDirectory = todayDirectory;
        }

        private protected override void CreateHtmlFile()
        {
            string fileName;

            File.Copy(Path.Combine(ServiceProvider.ReportsDirectory, "transacts-history-template.html"),
                                   Path.Combine(_reportTargetDirectory, fileName).Replace("json", "html"), true);
        }

        private protected override void CreateJsonFile()
        {
            string fileName;
            string serializedData;

            File.WriteAllText(Path.Combine(_reportTargetDirectory, fileName), serializedData);
        }


    }
}
