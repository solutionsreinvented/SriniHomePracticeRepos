using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class DraftingActivity : EngineeringActivity, IEngineeringActivity
    {
        public DraftingActivity(int activityId) : base(activityId)
        {
            ActivityType = EngineeringActivityType.Drafting;
        }
    }
}
