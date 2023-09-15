using System.ComponentModel;

namespace ReInvented.ExcelInteropDesign.Enums
{
    public enum TransverseWebStiffeners
    {
        Yes,
        No
    }
    public enum WebStiffenersConfiguration
    {
        [Description("One Side")]
        OneSide,
        [Description("Both Sides")]
        BothSides
    }
}
