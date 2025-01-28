using System;
using System.Collections.Generic;
using System.IO;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Services;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Optimization.Models
{
    public class PlatesOptimizationReport : ValidatablePropertyStore
    {
        #region Default Constructor

        public PlatesOptimizationReport()
        {
            Criteria = new PlateOptimizationCriteria();
        }

        #endregion

        #region Public Properties

        public string SourceStaadFile { get => Get<string>(); set { Set(value); UpdateOutputFiles(); } }

        public PlateOptimizationCriteria Criteria { get => Get<PlateOptimizationCriteria>(); set => Set(value); }

        public OutputFiles OutputFiles { get; private set; }

        public HashSet<PlateGroupDesignResult> Results { get; set; }

        #endregion

        #region Public Functions

        public void Save()
        {
            Save(this, OutputFiles.JsonOutputFileFullPath);
        }

        #endregion


        #region Public Static Functions

        public static void Save(PlatesOptimizationReport report, string outputFileFullPath)
        {
            JsonDataSerializer<PlatesOptimizationReport> serializer = new JsonDataSerializer<PlatesOptimizationReport>();
            string serialized = "const content = " + serializer.Serialize(report, JsonSerializerSettingsProvider.Minified);

            File.WriteAllText(outputFileFullPath, serialized);
        }

        #endregion

        #region Private Helpers

        private void UpdateOutputFiles()
        {
            OutputFiles = new OutputFiles(SourceStaadFile);
        }

        #endregion
    }
}
