using ReInvented.Domain.Design.ExcelInterop.Base;
using ReInvented.Domain.Design.ExcelInterop.Interfaces;

namespace ReInvented.Domain.Design.ExcelInterop.Models
{
    public sealed class BoxOrOSectionAxialStrengthParameters : AxialStrengthParameters, IAxialStrengthParameters, IGussetParameters
    {
        public double Tg { get => Get<double>(); set => Set(value); }

        public double Lcon { get => Get<double>(); set => Set(value); }
    }
}
