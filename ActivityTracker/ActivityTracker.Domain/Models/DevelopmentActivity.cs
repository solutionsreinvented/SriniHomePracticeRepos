using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Domain.Models
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
