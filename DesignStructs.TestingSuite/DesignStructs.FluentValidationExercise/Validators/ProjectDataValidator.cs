
using FluentValidation;
using ReInvented.Domain.ProjectSetup.Interfaces;
using DesignStructs.FluentValidationExercise.Extensions;
using System.IO;

namespace ReInvented.FluentValidationExercise.Validators
{
    public class ProjectDataValidator : AbstractValidator<IProjectData>
    {
        public ProjectDataValidator()
        {
            RuleFor(pd => pd.ProjectDirectory).NotEmpty("Project Directory")
                                              .Must(directory => Directory.Exists(directory)).WithMessage("Project directory does not exist. Please select a valid directory!");
            RuleFor(pd => pd.Structure).NotEmpty("Structure");
            RuleFor(pd => pd.Name).NotEmpty("Proejct Name");
            RuleFor(pd => pd.Client).NotEmpty("Client Name");
            RuleFor(pd => pd.Code).NotEmpty("Code")
                                  .Must((pd, code) => RegexService.MatchProjectCode(code, pd.Type)).WithMessage("Invalid project code for the selected project type");
        }
    }

}
