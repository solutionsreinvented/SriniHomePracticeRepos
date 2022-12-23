using PerformanceManager.Domain.Converters;
using System.ComponentModel;

namespace PerformanceManager.Domain.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ProjectType
    {
        [Description("Pre Order")]
        PreOrder,
        [Description("Order")]
        Order,
        [Description("Development")]
        Development
    }
}
