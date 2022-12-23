using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class DesignActivity : Activity, IActivity
    {
        #region Default Constructor

        public DesignActivity()
        {
            Discipline = Discipline.Design;
        }

        #endregion
    }
}
