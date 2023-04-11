using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Domain.Models
{
    public sealed class Engineer : Resource, IResource
    {
        public Engineer(int employeeId) : base(employeeId)
        {
            ResourceRole = ResourceRole.Engineer;
        }
    }
}
