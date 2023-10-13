using System;
using System.ComponentModel;

using Newtonsoft.Json;

using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interactivity.Enums;

namespace ReInvented.Domain.ProjectSetup.Models
{
    public class Settings : ErrorsEnabledPropertyStore, ISettings
    {
        #region Default Constructor

        public Settings()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        [JsonProperty]
        public IProjectInfo ProjectInfo { get => Get<IProjectInfo>(); private set => Set(value); }

        [JsonProperty]
        public ITimeOutSettings TimeOutSettings { get => Get<ITimeOutSettings>(); private set => Set(value); }

        [JsonProperty]
        public IAnalysisSettings AnalysisSettings { get => Get<IAnalysisSettings>(); private set => Set(value); }

        [JsonProperty]
        public IDesignSettings DesignSettings { get => Get<IDesignSettings>(); private set => Set(value); }

        [JsonProperty]
        public IReportSettings ReportSettings { get => Get<IReportSettings>(); private set => Set(value); }

        [JsonProperty]
        public IPerformanceSettings PerformanceSettings { get => Get<IPerformanceSettings>(); private set => Set(value); }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            ProjectInfo = new ProjectInfo();
            TimeOutSettings = new TimeOutSettings();
            AnalysisSettings = new AnalysisSettings();
            DesignSettings = new DesignSettings();
            ReportSettings = new ReportSettings();
            PerformanceSettings = new PerformanceSettings();

            AttachEvents();
        }

        private void AttachEvents()
        {
            if (DesignSettings != null)
            {
                DesignSettings.PropertyChanged -= OnDesignSettingsPropertyChanged;
                DesignSettings.PropertyChanged += OnDesignSettingsPropertyChanged;
            }
            if (AnalysisSettings != null)
            {
                AnalysisSettings.PropertyChanged -= OnAnalysisSettingsPropertyChanged;
                AnalysisSettings.PropertyChanged += OnAnalysisSettingsPropertyChanged;
            }
        }

        private void OnDesignSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DesignSettings.PerformAutoDesign))
            {
                if (ReportSettings != null && !DesignSettings.PerformAutoDesign)
                {
                    ReportSettings.GenerateMTO = false;
                    DesignSettings.CheckNonPreferredSectionsAlso = false;
                    DesignSettings.LimitDesignIterations = false;
                }
            }
        }
        private void OnAnalysisSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AnalysisSettings.WaitMode))
            {
                if (DesignSettings != null && AnalysisSettings.WaitMode != WaitAnalysisMode.Wait)
                {
                    DesignSettings.PerformAutoDesign = false;
                }
            }
        }

        #endregion
    }
}
