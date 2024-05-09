
using FluentValidation;

using ReInvented.Domain.Reporting.Models;

namespace ReInvented.FluentValidationExercise.Validators
{
    public class ContingenciesValidator : AbstractValidator<Contingencies>
    {
        public ContingenciesValidator()
        {
            RuleFor(c => c.Connections).GreaterThanOrEqualTo(0).WithMessage("Contingency for connections shall be greater than or equal to zero");
            RuleFor(c => c.Sections).GreaterThanOrEqualTo(0).WithMessage("Contingency for sections shall be greater than or equal to zero");
            RuleFor(c => c.Plates).GreaterThanOrEqualTo(0).WithMessage("Contingency for plates shall be greater than or equal to zero");
            RuleFor(c => c.BoltedFlanges).GreaterThanOrEqualTo(0).WithMessage("Contingency for bolted flanges shall be greater than or equal to zero");
        }
    }

}
