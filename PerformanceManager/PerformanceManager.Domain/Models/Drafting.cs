using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class Drafting : Activity, IActivity
    {
        public Drafting(int activityId) : base(activityId)
        {
            ActivityType = ActivityType.Detailing;
        }
    }
}
