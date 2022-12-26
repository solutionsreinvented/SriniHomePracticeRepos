using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
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
