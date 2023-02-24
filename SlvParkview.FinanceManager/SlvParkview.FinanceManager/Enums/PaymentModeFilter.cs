using System.ComponentModel;

using ReInvented.Shared.TypeConverters;

namespace SlvParkview.FinanceManager.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum PaymentModeFilter
    {
        All,
        Cash,
        Online,
        Cheque,
        [Description("App Payment")]
        AppPayment
    }
}
