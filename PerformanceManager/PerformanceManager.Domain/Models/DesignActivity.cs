using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class DesignActivity : Activity, IActivity
    {
        public DesignActivity(int id) : base(id)
        {
            ActivityType = ActivityType.Design;
        }
    }
}
