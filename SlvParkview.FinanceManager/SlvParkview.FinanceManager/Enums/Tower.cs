using ReInvented.Shared.TypeConverters;

using System.ComponentModel;

namespace SlvParkview.FinanceManager.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum Tower
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4
    }
}
