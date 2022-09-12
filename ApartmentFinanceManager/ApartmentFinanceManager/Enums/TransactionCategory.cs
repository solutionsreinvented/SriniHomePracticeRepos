using ReInvented.Shared.TypeConverters;

using System.ComponentModel;

namespace ApartmentFinanceManager.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum TransactionCategory
    {
        [Description("One Time Charges")]
        OneTimeCharges,

        [Description("Monthly Maintenance Charges")]
        Maintenance
    }
}
