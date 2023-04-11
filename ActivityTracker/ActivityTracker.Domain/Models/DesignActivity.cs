using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Domain.Models
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
