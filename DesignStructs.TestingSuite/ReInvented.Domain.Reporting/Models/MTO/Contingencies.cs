using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class Contingencies : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public Contingencies()
        {

        }

        #endregion

        #region Public Properties

        public double Connections { get => Get<double>(); set => Set(value); }

        public double Sections { get => Get<double>(); set => Set(value); }

        public double Plates { get => Get<double>(); set => Set(value); }

        #endregion
    }
}
