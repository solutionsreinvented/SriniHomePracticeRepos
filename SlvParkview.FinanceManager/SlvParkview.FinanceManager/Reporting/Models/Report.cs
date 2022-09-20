using Newtonsoft.Json;

using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Services;

using System;
using System.IO;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    public abstract class Report : PropertyStore, IReport
    {
        #region Private Fields

        private protected string _reportTargetDirectory;

        #endregion

        #region Default Constructor

        public Report()
        {

        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public string GeneratedOn => DateTime.Today.ToString("dd MMM yyyy");

        [JsonProperty]
        public string ApartmentName => "SLV Parkview Apartment";

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the contents of for the selected type of reports.
        /// </summary>
        public abstract void GenerateContents();

        /// <summary>
        /// Creates the required directories for generating various reports and then finally generates the report itself in HTML format.
        /// Creates a Json file also to store the data of the report.
        /// </summary>
        public virtual void SaveReport()
        {
            ///GenerateContents();
            CreateRequiredDirectories();
            CreateJsonFile();
            CreateHtmlFile();
        }


        #endregion

        #region Private Helper Functions

        /// <summary>
        /// Serializes the contents of 'this' report object and provides the same for persistence.
        /// </summary>
        /// <returns>A serialized string</returns>
        private protected abstract string GetSerializedData();

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Creates the main Reports directory and other required directories dependin on the type of report selected
        /// if they do not exist.
        /// </summary>
        private protected virtual void CreateRequiredDirectories()
        {
            /// Create if the Reports directory does not exists.
            if (!Directory.Exists(ServiceProvider.ReportsDirectory))
            {
                _ = Directory.CreateDirectory(ServiceProvider.ReportsDirectory);
            }
        }

        /// <summary>
        /// Creates the HTML file using the templates.
        /// </summary>
        private protected abstract void CreateHtmlFile();

        private protected abstract void CreateJavaScriptFile();

        /// <summary>
        /// Creates and stores the data of this report in a Json file.
        /// </summary>
        private protected abstract void CreateJsonFile();

        #endregion

    }
}
