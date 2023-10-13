using ReInvented.FluentValidationExercise.Validators;
using ReInvented.Shared.Stores;

namespace ReInvented.FluentValidationExercise.Models
{
    public class FeedWell : ErrorsEnabledPropertyStore
    {
        public FeedWell(Input input)
        {
            Input = input;
        }

        public double Diameter { get => Get<double>(); set => Set(value); }

        public double SubmergedHeight { get => Get<double>(); set => Set(value); }

        public Input Input { get; private set; }
    }
}
