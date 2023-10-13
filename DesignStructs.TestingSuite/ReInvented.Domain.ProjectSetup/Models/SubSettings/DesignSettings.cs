using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.ProjectSetup.Models
{
    public class DesignSettings : ErrorsEnabledPropertyStore, IDesignSettings
    {
        #region Default Constructor

        public DesignSettings()
        {

        }

        #endregion

        #region Public Properties

        public bool PerformAutoDesign { get => Get<bool>(); set => Set(value); }

        public bool CheckNonPreferredSectionsAlso { get => Get<bool>(); set => Set(value); }

        public bool LimitDesignIterations { get => Get<bool>(); set => Set(value); }

        public int DesignIterationsLimit { get => Get<int>(); set => Set(value); }

        #endregion
    }
}
