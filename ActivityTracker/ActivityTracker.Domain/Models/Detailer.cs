using ProdActivity.Domain.Base;
using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;

namespace ProdActivity.Domain.Models
{
    public sealed class Detailer : Resource, IResource
    {
        public Detailer(int employeeId) : base(employeeId)
        {
            ResourceRole = ResourceRole.Drafter;
        }
    }
}
