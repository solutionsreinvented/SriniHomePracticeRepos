using ProdActivity.Domain.Base;
using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;

namespace ProdActivity.Domain.Models
{
    public class DevelopmentActivity : Activity, IActivity
    {
        #region Default Constructor

        public DevelopmentActivity(IProject selectedProject) : base(selectedProject)
        {
            Discipline = Discipline.Development;
        }

        #endregion
    }
}
