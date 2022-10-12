using ReInvented.Shared.TypeConverters;

using System.ComponentModel;

namespace SlvParkview.FinanceManager.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum TransactionCategory
    {
        [Description("One Time Charges")]
        OneTimeCharges,

        [Description("Maintenance Charges")]
        Maintenance
    }
}
