using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class Engineer : Resource, IResource
    {
        public Engineer(int employeeId) : base(employeeId)
        {
            ResourceRole = ResourceRole.Engineer;
        }
    }
}
