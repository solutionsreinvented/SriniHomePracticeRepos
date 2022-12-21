using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class Design : Activity, IActivity
    {
        public Design(int id) : base(id)
        {
            ActivityType = ActivityType.Design;
        }

    }
}
