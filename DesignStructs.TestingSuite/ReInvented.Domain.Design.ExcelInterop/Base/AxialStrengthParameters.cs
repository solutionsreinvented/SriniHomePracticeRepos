using ReInvented.Domain.Design.ExcelInterop.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Design.ExcelInterop.Base
{
    public abstract class AxialStrengthParameters : ErrorsEnabledPropertyStore, IAxialStrengthParameters
    {
        public double Lus { get => Get<double>(); set => Set(value); }
    }
}
