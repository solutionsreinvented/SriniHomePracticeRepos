using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.ExcelInteropDesign.Base
{
    public abstract class AxialStrengthParameters : ErrorsEnabledPropertyStore, IAxialStrengthParameters
    {
        public double Lus { get => Get<double>(); set => Set(value); }
    }
}
