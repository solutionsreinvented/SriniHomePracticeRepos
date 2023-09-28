using ReInvented.Documentation.Reporting.Interfaces;
using ReInvented.Shared.Stores;
using ReInvented.TestingSuite;

namespace ReInvented.Documentation.Reporting.Models
{
    public class ProjectSettings : ErrorsEnabledPropertyStore
    {
        public ProjectSettings()
        {
            Project = new Project();

            CanStartStaadApplication = false;
            FileOpeningTimeout = 10.0;
            AnalysisTimeout = 120.0;
            AutoDesignTimeout = 540.0;
            ModelAvailabilityCheckInterval = 0.5;
        }

        public IProject Project { get => Get<IProject>(); set => Set(value); }

        #region Staad Limits

        public double FileOpeningTimeout { get => Get<double>(); set => Set(value); }

        public double AnalysisTimeout { get => Get<double>(); set => Set(value); }

        public double AutoDesignTimeout { get => Get<double>(); set => Set(value); }

        public double ModelAvailabilityCheckInterval { get => Get<double>(); set => Set(value); }

        #endregion

        #region Staad Operations

        public bool CanStartStaadApplication { get => Get<bool>(); set => Set(value); }

        public SilentMode SilentMode { get => Get<SilentMode>(); set => Set(value); }

        public HiddenMode HiddenMode { get => Get<HiddenMode>(); set => Set(value); }

        public WaitMode WaitMode { get => Get<WaitMode>(); set => Set(value); }

        #endregion

        #region Design and Reporting

        public bool PerformAutoDesign
        {
            get => Get<bool>();
            set
            {
                Set(value);
                if (!value)
                {
                    GenerateMTO = false;
                    CheckNonPreferredSectionsAlso = false;
                }
            }
        }

        public bool GenerateMTO
        {
            get => Get<bool>();
            set
            {
                Set(value);
                if (!value)
                {
                    IncludeMTONotes = false;
                }
            }
        }

        public bool IncludeMTONotes { get => Get<bool>(); set => Set(value); }

        public bool CheckNonPreferredSectionsAlso { get => Get<bool>(); set => Set(value); }

        public bool GenerateContentLoadsCalculationReport { get => Get<bool>(); set => Set(value); }

        public bool GenerateStructureSeismicCalculationReport { get => Get<bool>(); set => Set(value); }


        #endregion


    }
}
