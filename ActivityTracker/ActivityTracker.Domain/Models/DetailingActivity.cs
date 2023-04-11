using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Domain.Models
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
