using System.ComponentModel;

using ReInvented.Shared.TypeConverters;

namespace ReInvented.Documentation.Reporting.Enums
{
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum ProjectType
    {
        [Description("Enquiry")]
        Enquiry,
        [Description("Order")]
        Order,
        [Description("Development")]
        Development
    }
}
