using ProdActivity.Domain.Base;
using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;

namespace ProdActivity.Domain.Models
{
    public sealed class DesignActivity : Activity, IActivity
    {
        #region Default Constructor

        public DesignActivity(IProject selectedProject) : base(selectedProject)
        {
            Discipline = Discipline.Design;
        }

        #endregion
    }
}
