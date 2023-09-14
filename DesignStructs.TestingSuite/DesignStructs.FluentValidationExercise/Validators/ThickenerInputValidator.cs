using DesignStructs.FluentValidationExercise.Models;

using FluentValidation;

namespace DesignStructs.FluentValidationExercise.Validators
{
    public class ThickenerInputValidator : AbstractValidator<ThickenerInput>
    {
        public ThickenerInputValidator()
        {
            RuleFor(i => i.Shell.Diameter)
                .GreaterThan(0.0).WithMessage("Shell diameter shall be greater than zero")
                .GreaterThan(i => i.FeedWell.Diameter).WithMessage("Shell diameter shall be greater than the feedwell diameter");
        }
    }
}
