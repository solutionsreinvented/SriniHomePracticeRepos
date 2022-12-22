using System.ComponentModel;

using ReInvented.Shared.TypeConverters;

namespace Techture.PerformanceManager.Enums
{
    public enum ProjectType
    {
        PreOrder,
        Order
    }
    public enum ActivityType
    {
        Design,
        Detailing
    }

    public enum StructurePart
    {
        NonStructural,
        Bridge,
        Tank,
        SupportStructure,
        RakeMechanism
    }

    [TypeConverter(typeof(EnumToDescriptionTypeConverter))]
    public enum DesignActivityCategory
    {
        [Description("Preamble")]
        Preamble,
        [Description("Foundation Load Data")]
        FoundationLoadData,
        [Description("Weights Estimation")]
        WeightsEstimation,
        [Description("Base Plates Design")]
        BasePlatesDesign,
        [Description("Geometry Modeling")]
        GeometryModeling,
        [Description("Loads Application")]
        LoadsApplication,
        [Description("Analysis and Design")]
        AnalysisAndDesign,
        [Description("Member Design Calculation Report")]
        MemberDesignCalculationReport,
        [Description("Connections Calculation Report")]
        ConnectionsCalculationReport
    }

}
