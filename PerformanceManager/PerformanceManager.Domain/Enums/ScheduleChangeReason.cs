using System.ComponentModel;

namespace PerformanceManager.Domain.Enums
{
    public enum ScheduleChangeReason
    {
        [Description("Change in Priority")]
        PriorityChange,
        [Description("Structural Input Pending")]
        StructuralInputPending,
        [Description("Mechanical Input Pending")]
        MechanicalInputPending
    }
}
