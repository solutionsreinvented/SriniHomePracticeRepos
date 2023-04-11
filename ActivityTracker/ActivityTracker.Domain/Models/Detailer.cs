using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Domain.Models
{
    public sealed class Detailer : Resource, IResource
    {
        public Detailer(int employeeId) : base(employeeId)
        {
            ResourceRole = ResourceRole.Drafter;
        }
    }
}
