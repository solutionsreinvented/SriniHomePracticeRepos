using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class Detailer : Resource, IResource
    {
        public Detailer(int employeeId) : base(employeeId)
        {
            ResourceRole = ResourceRole.Drafter;
        }
    }
}
