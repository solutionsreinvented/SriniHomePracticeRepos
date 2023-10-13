using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.ProjectSetup.Models
{
    public class TimeOutSettings : ErrorsEnabledPropertyStore, ITimeOutSettings
    {
        #region Default Constructor

        public TimeOutSettings()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public double FileOpeningTimeout { get => Get<double>(); set => Set(value); }

        public double AnalysisTimeout { get => Get<double>(); set => Set(value); }

        public double AutoDesignTimeout { get => Get<double>(); set => Set(value); }

        public double ModelAvailabilityCheckInterval { get => Get<double>(); set => Set(value); }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            FileOpeningTimeout = 10.0;
            AnalysisTimeout = 120.0;
            AutoDesignTimeout = 540.0;
            ModelAvailabilityCheckInterval = 0.5;
        }

        #endregion
    }
}
