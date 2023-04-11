using System.ComponentModel;

namespace ActivityTracker.Domain.Enums
{
    public enum ScheduleChangeReason
    {
        [Description("Change in Priority")]
        PriorityChange,
        [Description("Delay in Structural Input")]
        DelayInStructuralInput,
        [Description("Delay in Mechanical Input")]
        DelayInMechanicalInput
    }
}
