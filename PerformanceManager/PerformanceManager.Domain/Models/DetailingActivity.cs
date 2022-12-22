using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class DetailingActivity : Activity, IActivity
    {
        public DetailingActivity(int activityId) : base(activityId)
        {
            ActivityType = Enums.Domain.Detailing;
            ProjectType = ProjectType.Order;
        }
    }
}
