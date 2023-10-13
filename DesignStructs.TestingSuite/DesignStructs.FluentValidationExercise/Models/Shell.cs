using System.ComponentModel;
using System.Runtime.CompilerServices;

using ReInvented.FluentValidationExercise.Validators;
using ReInvented.Shared.Stores;

namespace ReInvented.FluentValidationExercise.Models
{
    public class Shell : ErrorsEnabledPropertyStore, INotifyDataErrorInfo
    {
        public Shell(Input input)
        {
            Input = input;
        }

        public double Diameter { get => Get<double>(); set => Set(value); }

        public double Swd { get => Get<double>(); set => Set(value); }

        public Input Input { get; private set; }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName == nameof(Diameter))
            {
                if (Diameter <= 0.0)
                {
                    NotifyDataErrorInfo.ValidateProperty("Shell diameter shall be greater than 0.0", propertyName);
                }
            }
            base.OnPropertyChanged(propertyName);
        }

    }
}
