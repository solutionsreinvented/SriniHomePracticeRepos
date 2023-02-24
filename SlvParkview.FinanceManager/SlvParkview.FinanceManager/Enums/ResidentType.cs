using System.ComponentModel;

using ReInvented.Shared.TypeConverters;

namespace SlvParkview.FinanceManager.Enums
{
    /// <summary>
    /// Indicates the type of the resident staying in a specified flat.
    /// </summary>
    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum ResidentType
    {
        /// <summary>
        /// Owner of the flat himself/herself.
        /// </summary>
        Owner,
        /// <summary>
        /// A tenant.
        /// </summary>
        Tenant
    }
}
