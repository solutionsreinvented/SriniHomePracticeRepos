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
        Maintenance,
        [Description("Penalty for Delay in Maintenance Payment")]
        MaintenancePaymentDelay,
        [Description("Wrong Parking In Common Area")]
        WrongParkingInCommonArea,
        [Description("Wrong Parking In Flat Parking Access Way")]
        WrongParkingInFlatParkingAccessWay
    }
}
