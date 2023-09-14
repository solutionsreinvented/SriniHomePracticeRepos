using ReInvented.Shared.Stores;

namespace DesignStructs.FluentValidationExercise.Models
{
    public class Shell : ErrorsEnabledPropertyStore
    {
        public Shell()
        {

        }

        public double Diameter { get => Get<double>(); set => Set(value); }

        public double Swd { get => Get<double>(); set => Set(value); }
    }
}
