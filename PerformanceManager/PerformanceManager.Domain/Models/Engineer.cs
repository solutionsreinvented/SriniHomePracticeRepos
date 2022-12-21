using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class Engineer : EngineeringResource, IEngineeringResource
    {
        public Engineer(int employeeId) : base(employeeId)
        {
            ResourceRole = ResourceRole.Engineer;
        }
    }
}
