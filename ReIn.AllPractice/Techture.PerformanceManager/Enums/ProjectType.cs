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

    public enum DesignActivityClassification
    {
        Estimation,
        DetaileDesign
    }
    public enum EstimationActivityClassification
    {
        Bridge,
        Tank,
        TankAndSupportStructure,
        RakeMechanism,
        BridgeTankAndSupportStructure,
        CompleteThickener
    }
    public enum DetailedDesignClassification
    {
        BridgeMemberDesign,
        TankAndSupportStructureMemberDesign,

        MemberDesignCalculationReport,
        DetaileDesign
    }

}
