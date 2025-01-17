using ProdActivity.Domain.Base;
using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;

namespace ProdActivity.Domain.Models
{
    public sealed class DetailingActivity : Activity, IActivity
    {
        #region Default Constructor

        public DetailingActivity(IProject selectedProject) : base(selectedProject)
        {
            Discipline = Discipline.Detailing;
        }

        #endregion
    }
}
