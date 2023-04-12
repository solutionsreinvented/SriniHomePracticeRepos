
using ActivityTracker.Domain.Enums;

namespace ActivityTracker.Domain.Models
{
    public class PreOrder : Project
    {
        public PreOrder()
        {
            Type = ProjectType.PreOrder;
        }

        #region Parameterized Constructors

        public PreOrder(string code, string name) : base(code, name)
        {
            Type = ProjectType.PreOrder;
        }

        #endregion
    }
}
