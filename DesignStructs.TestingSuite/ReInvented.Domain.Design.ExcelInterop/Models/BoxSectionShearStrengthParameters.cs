using ReInvented.Domain.Design.ExcelInterop.Enums;
using ReInvented.Domain.Design.ExcelInterop.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Design.ExcelInterop.Models
{
    public sealed class BoxSectionShearStrengthParameters : ErrorsEnabledPropertyStore, IShearStrengthParameters
    {
        #region Default Constructor

        public BoxSectionShearStrengthParameters()
        {
            HssForm = HssForm.SAW;
            GussetConnectedTo = GussetToBoxSectionConnectionSide.Longer;
            GussetConfiguration = GussetToBoxSectionConfiguration.SingleConcentric;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates how the HSS section is produced.
        /// </summary>
        public HssForm HssForm { get => Get<HssForm>(); set => Set(value); }
        /// <summary>
        /// Indicates where the gusset plate is connected on the box section.
        /// </summary>
        public GussetToBoxSectionConnectionSide GussetConnectedTo { get => Get<GussetToBoxSectionConnectionSide>(); set => Set(value); }
        /// <summary>
        /// Indicates how many gusssets are provided and how are they provided to connect to box section.
        /// </summary>
        public GussetToBoxSectionConfiguration GussetConfiguration { get => Get<GussetToBoxSectionConfiguration>(); set => Set(value); }

        #endregion
    }
}
