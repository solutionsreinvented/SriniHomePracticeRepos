
using ActivityTracker.Domain.Enums;

namespace ActivityTracker.Domain.Models
{
    public class Order : Project
    {
        public Order()
        {
            Type = ProjectType.Order;
        }

        #region Parameterized Constructors

        public Order(string code, string name) : base(code, name)
        {
            Type = ProjectType.Order;
        }

        #endregion
    }
}
