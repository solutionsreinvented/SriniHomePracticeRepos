using ReInvented.Shared.Stores;

namespace DesignStructs.FluentValidationExercise.Models
{
    public class Feedwell : ErrorsEnabledPropertyStore
    {
        public Feedwell()
        {

        }

        public double Diameter { get => Get<double>(); set => Set(value); }

        public double SubmergedHeight { get => Get<double>(); set => Set(value); }

    }
}
