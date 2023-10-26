using System.ComponentModel;

namespace ReInvented.Domain.Reporting.Enums
{
    public enum SubmissionCategory
    {
        [Description("For Information")]
        Information,
        [Description("For Review & Approval")]
        Approval,
        [Description("For Use/Implementation")]
        Implementation
    }
}
