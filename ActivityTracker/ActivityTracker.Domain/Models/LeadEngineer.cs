using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Domain.Models
{
    public sealed class LeadEngineer : Resource, IResource
    {
        public LeadEngineer(int employeeId) : base(employeeId)
        {
            ResourceRole = ResourceRole.LeadEngineer;
        }
    }
}
