using ReInvented.FluentValidationExercise.Models;

using FluentValidation;

namespace ReInvented.FluentValidationExercise.Validators
{
    public class ThickenerInputValidator : AbstractValidator<Input>
    {
        public ThickenerInputValidator()
        {
            _ = RuleFor(i => i.Shell.Diameter)
                    .GreaterThan(0).WithMessage("Shell diameter shall be greater than zero")
                    .GreaterThan(i => i.FeedWell.Diameter).WithMessage("Shell diameter shall be greater than the feedwell diameter");
            _ = RuleFor(i => i.FeedWell.Diameter)
                    .GreaterThan(0.0).WithMessage("Feedwell diameter shall be greater than zero")
                    .LessThan(i => i.Shell.Diameter).WithMessage("Feedwell diameter shall be less than shell diameter");
        }
    }
}
