using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interactivity.Enums;

namespace ReInvented.Domain.ProjectSetup.Models
{
    public class AnalysisSettings : ErrorsEnabledPropertyStore, IAnalysisSettings
    {
        #region Default Constructor

        public AnalysisSettings()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public bool CanStartStaadApplication { get => Get<bool>(); set => Set(value); }

        public SilentMode SilentMode { get => Get<SilentMode>(); set => Set(value); }

        public HiddenMode HiddenMode { get => Get<HiddenMode>(); set => Set(value); }

        public WaitAnalysisMode WaitMode { get => Get<WaitAnalysisMode>(); set => Set(value); }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            CanStartStaadApplication = false;
            WaitMode = WaitAnalysisMode.Wait;
        }

        #endregion
    }
}
