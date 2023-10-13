using ReInvented.Domain.Design.ExcelInterop.Enums;
using ReInvented.Domain.Design.ExcelInterop.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Design.ExcelInterop.Models
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
