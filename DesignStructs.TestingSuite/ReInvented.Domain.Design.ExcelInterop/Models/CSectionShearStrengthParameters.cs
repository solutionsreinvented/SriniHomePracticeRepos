using ReInvented.Domain.Design.ExcelInterop.Enums;
using ReInvented.Domain.Design.ExcelInterop.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Design.ExcelInterop.Models
{
    public sealed class CSectionShearStrengthParameters : ErrorsEnabledPropertyStore, IShearStrengthParameters
    {
        #region Default Constructor

        public CSectionShearStrengthParameters()
        {
            StiffenersConfiguration = WebTransverseStiffenersConfiguration.OneSide;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Configuration of the web stiffeners i.e., whether stiffeners are provided on one side or both sides or none. For C sections this can be either None or OneSide.
        /// Note: If BothSides is selected it will be defaulted to OneSide.
        /// </summary>
        public WebTransverseStiffenersConfiguration StiffenersConfiguration
        {
            get => Get<WebTransverseStiffenersConfiguration>();
            set
            {
                if (value == WebTransverseStiffenersConfiguration.BothSides)
                {
                    Set(WebTransverseStiffenersConfiguration.OneSide);
                }
                else
                {
                    Set(value);
                }
            }
        }
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
