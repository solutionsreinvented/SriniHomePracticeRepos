using ReInvented.ExcelInteropDesign.Base;
using ReInvented.ExcelInteropDesign.Interfaces;

namespace ReInvented.ExcelInteropDesign.Models
{
    public sealed class HSectionAxialStrengthParameters : AxialStrengthParameters, IAxialStrengthParameters
    {
        /// <summary>
        /// Net sectional area for the tensile strength calculations.
        /// </summary>
        public double Anet { get => Get<double>(); set => Set(value); }
        /// <summary>
        /// Torsional unbraced length of the member
        /// </summary>
        public double Ltub => Lus / 10.0;
    }
}
