using ReInvented.ExcelInteropDesign.Enums;
using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.ExcelInteropDesign.Models
{
    public sealed class OSectionShearStrengthParameters : ErrorsEnabledPropertyStore, IShearStrengthParameters
    {
        #region Default Constructor

        public OSectionShearStrengthParameters()
        {
            HssForm = HssForm.SAW;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates how the HSS section is produced.
        /// </summary>
        public HssForm HssForm { get => Get<HssForm>(); set => Set(value); }

        #endregion
    }
}
