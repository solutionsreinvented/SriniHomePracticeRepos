﻿using SlvParkview.FinanceManager.Enums;

using System;

namespace SlvParkview.FinanceManager.Reporting.Interfaces
{
    public interface IReportOptions
    {
        event Action ReportOptionChanged;

        PaymentModeFilter PaymentModeFilter { get; set; }
    }
}
