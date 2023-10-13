using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.ProjectSetup.Models
{
    public class ReportSettings : ErrorsEnabledPropertyStore, IReportSettings
    {
        #region Default Constructor

        public ReportSettings()
        {

        }

        #endregion

        #region Public Properties

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

        public bool GenerateContentLoadsCalculationReport { get => Get<bool>(); set => Set(value); }

        public bool GenerateStructureSeismicCalculationReport { get => Get<bool>(); set => Set(value); }

        #endregion
    }
}
