using Newtonsoft.Json;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Models.Base;
using SlvParkview.FinanceManager.Services;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    /// <summary>
    /// Creates a report which comprises of the payments received from each flat (of a given block) in a specified month.
    /// </summary>
    public abstract class PaymentsReport : Report, IReport
    {
        #region Private Fields

        private protected readonly Block _block;

        private protected readonly IReportOptions _reportOptions;

        #endregion

        #region Default Constructor

        protected PaymentsReport()
        {

        }

        #endregion

        #region Parameterized Constructor

        public PaymentsReport(Block block, IReportOptions reportOptions)
        {
            _block = block;
            _reportOptions = reportOptions;
        }

        #endregion

        #region Read-only Properties

        [JsonProperty]
        public List<PaymentInfo> Payments { get => Get<List<PaymentInfo>>(); private protected set => Set(value); }

        [JsonProperty]
        public string TotalPayment { get => Get<string>(); private protected set => Set(value); }

        /// Exercise cuation while changing this string. This string is used to access the linked js files.
        private protected override string TemplateFileName => GetTemplateFileName();
        #endregion

        #region Private Helper Methods

        private protected override void CreateRequiredDirectories()
        {
            base.CreateRequiredDirectories();

            /// Create a new directory to store Monthwise Payments reports if it does not exists.

            _reportTargetDirectory = Path.Combine(FileServiceProvider.PaymentsReportsDirectory);

            if (!Directory.Exists(_reportTargetDirectory))
            {
                _ = Directory.CreateDirectory(_reportTargetDirectory);
            }
        }

        #endregion

        #region Abstract Members

        private protected abstract void SetPrivateProperties();

        private protected abstract string GetTemplateFileName();

        private protected virtual List<PaymentInfo> ApplyFilterOn(List<PaymentInfo> payments)
        {
            return _reportOptions.PaymentModeFilter == PaymentModeFilter.All
                ? payments
                : payments.Where(p => p.Mode == ConvertersService.PaymentModeConverter.ConvertToString(_reportOptions.PaymentModeFilter)).ToList();
        }

        #endregion

    }
}
