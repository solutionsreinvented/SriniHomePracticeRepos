using System.ComponentModel;

namespace ReInvented.Domain.Design.ExcelInterop.Enums
{
    /// <summary>
    /// Indicates how many gusssets are provided and how are they provided to connect to box section.
    /// </summary>
    public enum GussetToBoxSectionConfiguration
    {
        [Description("Single Concentric")]
        SingleConcentric,
        [Description("Two Side Gussets")]
        TwoSide
    }
}
