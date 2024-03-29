﻿using ReInvented.Shared.TypeConverters;

using SlvParkview.FinanceManager.Enums;

namespace SlvParkview.FinanceManager.Services
{
    public static class ConvertersService
    {
        public static readonly EnumToDescriptionTypeConverter TransactionCategoryConverter = new EnumToDescriptionTypeConverter(typeof(TransactionCategory));

        public static readonly EnumToDescriptionTypeConverter PaymentModeConverter = new EnumToDescriptionTypeConverter(typeof(PaymentMode));

        public static readonly EnumToDescriptionTypeConverter ReportTypeConverter = new EnumToDescriptionTypeConverter(typeof(ReportType));
    }
}
