using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class LeadEngineer : Resource, IResource
    {
        public LeadEngineer(int employeeId) : base(employeeId)
        {
            ResourceRole = ResourceRole.LeadEngineer;
        }
    }
}
