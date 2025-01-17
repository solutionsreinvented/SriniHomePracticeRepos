
using ProdActivity.Domain.Enums;

namespace ProdActivity.Domain.Models
{
    public class Development : Project
    {
        public Development()
        {
            Type = ProjectType.Development;
        }

        #region Parameterized Constructors

        public Development(string code, string name) : base(code, name)
        {
            Type = ProjectType.Development;
        }

        #endregion
    }
}
