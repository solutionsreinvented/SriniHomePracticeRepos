using ProdActivity.Domain.Converters;
using System.ComponentModel;

namespace ProdActivity.Domain.Enums
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
