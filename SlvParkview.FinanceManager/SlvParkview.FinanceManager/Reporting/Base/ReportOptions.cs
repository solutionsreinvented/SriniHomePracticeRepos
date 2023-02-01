﻿using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Reporting.Interfaces;

using System;

namespace SlvParkview.FinanceManager.Reporting.Models.Base
{
    public abstract class ReportOptions : PropertyStore, IReportOptions
    {
        #region Public Events

        public event Action ReportOptionChanged;

        #endregion

        #region Default Constructor

        public ReportOptions()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public ReceiptModeFilter ReceiptModeFilter { get => Get<ReceiptModeFilter>(); set { Set(value); RaiseReportOptionChanged(); } }

        #endregion

        #region Private Helpers

        private protected virtual void Initialize()
        {
            ReceiptModeFilter = ReceiptModeFilter.All;
        }

        private protected void RaiseReportOptionChanged()
        {
            ReportOptionChanged?.Invoke();
        }

        #endregion
    }
}
