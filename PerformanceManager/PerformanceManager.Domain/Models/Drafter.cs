using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class Drafter : EngineeringResource, IEngineeringResource
    {
        public Drafter(int employeeId) : base(employeeId)
        {
            ResourceRole = ResourceRole.Drafter;
        }


    }
}
