using Newtonsoft.Json;

using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Services;

using System.Collections.Generic;
using System.IO;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    /// <summary>
    /// Creates a report which comprises of the payments received from each flat (of a given block) in a specified month.
    /// </summary>
    public abstract class PaymentsReport : Report, IReport
    {
        #region Private Fields

        private protected readonly Block _block;

        #endregion

        #region Default Constructor

        protected PaymentsReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public PaymentsReport(Block block)
        {
            _block = block;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public List<PaymentInfo> Payments { get => Get<List<PaymentInfo>>(); private protected set => Set(value); }

        [JsonProperty]
        public string TotalPayment { get => Get<string>(); private protected set => Set(value); }

        #endregion

        #region Private Helper Methods

        private protected override void CreateRequiredDirectories()
        {
            base.CreateRequiredDirectories();

            /// Create a new directory to store Monthwise Payments reports if it does not exists.

            _reportTargetDirectory = Path.Combine(ServiceProvider.PaymentsReportsDirectory);

            if (!Directory.Exists(_reportTargetDirectory))
            {
                _ = Directory.CreateDirectory(_reportTargetDirectory);
            }
        }

        private protected override void CreateHtmlFile()
        {
            string fileName = $"{GetFileName()}.html";

            string[] htmlContents = File.ReadAllLines(Path.Combine(ServiceProvider.ReportTemplatesDirectory, $"{GetTemplateFileName()}.html"));

            List<string> finalHtmlFileContent = ConcatenateScriptTagIn(htmlContents, fileName);

            File.WriteAllLines(Path.Combine(_reportTargetDirectory, fileName), finalHtmlFileContent);
        }

        private protected override void CreateJavaScriptFile()
        {
            string fileName = $"{GetFileName()}.js";

            string jsFilePath = Path.Combine(ServiceProvider.ReportScriptsDirectory, $"{GetTemplateFileName()}.js");
            string[] jsContents = File.ReadAllLines(jsFilePath);

            string finalJavaScriptFileContent = ConcatenateJsonContentIn(jsContents);

            File.WriteAllText(Path.Combine(_reportTargetDirectory, fileName), finalJavaScriptFileContent);
        }

        #endregion

        #region Abstract Members

        private protected abstract void SetPrivateProperties();

        private protected abstract string GetTemplateFileName();

        #endregion

    }
}
