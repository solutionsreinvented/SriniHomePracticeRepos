using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class DesignActivity : EngineeringActivity, IEngineeringActivity
    {
        public DesignActivity(int activityId) : base(activityId)
        {
            ActivityType = EngineeringActivityType.Design;
        }

    }
}
