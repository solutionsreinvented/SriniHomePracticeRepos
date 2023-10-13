using ReInvented.Domain.Design.ExcelInterop.Enums;
using ReInvented.Domain.Design.ExcelInterop.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Design.ExcelInterop.Models
{
    public sealed class HSectionShearStrengthParameters : ErrorsEnabledPropertyStore, IShearStrengthParameters
    {
        #region Default Constructor

        public HSectionShearStrengthParameters()
        {
            StiffenersConfiguration = WebTransverseStiffenersConfiguration.BothSides;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Configuration of the web stiffeners i.e., whether stiffeners are provided on one side or both sides or none.
        /// </summary>
        public WebTransverseStiffenersConfiguration StiffenersConfiguration { get => Get<WebTransverseStiffenersConfiguration>(); set => Set(value); }
        /// <summary>
        /// Thickener of the stiffeners.
        /// </summary>
        public double Ts { get; set; }
        /// <summary>
        /// Center-to-center spacing of the stiffeners.
        /// </summary>
        public double Spacing { get; set; }

        #endregion
    }
}
