using ReInvented.ExcelInteropDesign.Base;
using ReInvented.ExcelInteropDesign.Interfaces;

namespace ReInvented.ExcelInteropDesign.Models
{
    public sealed class BoxOrOSectionAxialStrengthParameters : AxialStrengthParameters, IAxialStrengthParameters, IGussetParameters
    {
        public double Tg { get => Get<double>(); set => Set(value); }

        public double Lcon { get => Get<double>(); set => Set(value); }
    }
}
