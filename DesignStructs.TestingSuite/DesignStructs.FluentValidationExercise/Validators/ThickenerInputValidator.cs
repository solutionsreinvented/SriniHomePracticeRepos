using ReInvented.FluentValidationExercise.Models;

using FluentValidation;
using ReInvented.Domain.Tass.Common.Interfaces;
using ReInvented.Domain.ProjectSetup.Interfaces;
using DesignStructs.FluentValidationExercise.Extensions;
using ReInvented.Domain.ProjectSetup.Enums;
using System.Text.RegularExpressions;
using ReInvented.Domain.Reporting.Models;
using System;

namespace ReInvented.FluentValidationExercise.Validators
{

    public class RegexService
    {
        public static bool MatchProjectCode(string code, ProjectType projectType)
        {
            if (code == null)
            {
                return false;
            }

            Regex regex;

            if (projectType == ProjectType.Enquiry)
            {
                regex = new Regex(@"^E\d{7,8}(GS)?$");
                return regex.IsMatch(code.Trim());
            }
            else if (projectType == ProjectType.Order)
            {
                regex = new Regex(@"^\d{2}-\d{4}$");
                return regex.IsMatch(code.Trim());
            }
            else
            {
                return true;
            }
        }

        public static bool MatchDocumentNumber(string documentNumber, ProjectType projectType)
        {
            if (documentNumber == null)
            {
                return false;
            }

            Regex regex;

            if (projectType == ProjectType.Enquiry)
            {
                regex = new Regex(@"^E\d{7,8}(GS)?A0TR\d{3}[A-Za-z]{2}\d{3}$");
                return regex.IsMatch(documentNumber.Trim());
            }
            else if (projectType == ProjectType.Order)
            {
                regex = new Regex(@"^\d{4}A0TR\d{3}[A-Za-z]{2}\d{3}$");
                return regex.IsMatch(documentNumber.Trim());
            }
            else
            {
                return true;
            }
        }
    }

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

    public class ProjectValidator : AbstractValidator<IProject>
    {
        public ProjectValidator()
        {
            RuleFor(p => p.Input).SetValidator(new InputValidator()).When(p => p.Input != null);
            RuleFor(p => p.Settings).SetValidator(new SettingsValidator()).When(p => p.Settings != null);

            When(p => p.Settings.ReportSettings.GenerateMTO && p.MaterialTakeoffReport != null, () =>
            {
                RuleFor(p => p.MaterialTakeoffReport).SetValidator(new ReportValidator());
                RuleFor(p => (p.MaterialTakeoffReport as MTOReport).Contingencies).SetValidator(new ContingenciesValidator());
            });

            When(p => p.Settings.ReportSettings.GenerateFoundationLoadData && p.FoundationLoadDataReport != null, () =>
            {
                RuleFor(p => p.FoundationLoadDataReport).SetValidator(new ReportValidator()).When(p => p.Settings.ReportSettings.GenerateFoundationLoadData);
            });
        }
    }

    public class ReportValidator : AbstractValidator<IReport>
    {
        public ReportValidator()
        {
            RuleFor(r => r.Document).NotNull("Document").SetValidator(new DocumentValidator());
            RuleFor(r => r.ProjectData).NotNull("Project Data in Report");
            RuleFor(r => r.DataSource).NotNull("Report Data Source");
            RuleFor(r => r.FrontPageTitle).NotEmpty("Report front page title");
            RuleFor(r => r.Document.Number).NotEmpty("Document Number")
                                           .Must((r, number) => RegexService.MatchDocumentNumber(number, r.ProjectData.Type)).When(r => r.Document.Number != string.Empty)
                                           .WithMessage("The Document Number shall match with document number format for the selected project type.");
        }
    }
    public class DocumentValidator : AbstractValidator<IDocument>
    {
        public DocumentValidator()
        {
            RuleFor(d => d.Title).NotEmpty("Document title");
            RuleFor(d => d.Revisions).NotNull("Document revisions")
                                     .Must(revisions => revisions.Count >= 1).WithMessage("Revision history is empty. Add atleast one revision.")
                                     .ForEach(r => r.SetValidator(new RevisionValidator()));

        }
    }

    public class RevisionValidator : AbstractValidator<IRevision>
    {
        public RevisionValidator()
        {
            RuleFor(r => r.ScrutinyHistory).NotNull("Scrutiny History").SetValidator(r => new ScrutinyHistoryValidator(r.Code));
            RuleFor(r => r.RevisionDescriptionItems).NotNull("Revision descriptions").ForEach(rdi => rdi.SetValidator(new RevisionDescriptionItemValidator()));
        }
    }

    public class RevisionDescriptionItemValidator : AbstractValidator<IRevisionDescriptionItem>
    {
        public RevisionDescriptionItemValidator()
        {
            RuleFor(rdi => rdi.Section).NotEmpty("Revision description item - Section");
            RuleFor(rdi => rdi.Description).NotEmpty("Revision description item - Description");
        }
    }

    public class ScrutinyHistoryValidator : AbstractValidator<IScrutinyHistory>
    {
        public ScrutinyHistoryValidator(char revCode)
        {
            RuleFor(sh => sh.Originator).NotNull("Originator in Scrutiny History").SetValidator(new ScrutinizerValidator($"[Rev. {revCode}: Originator]"));
            RuleFor(sh => sh.Reviewer).NotNull("Reviewer in Scrutiny History").SetValidator(new ScrutinizerValidator($"[Rev. {revCode}: Reviewer]"));
            RuleFor(sh => sh.Approver).NotNull("Approver in Scrutiny History").SetValidator(new ScrutinizerValidator($"[Rev. {revCode}: Approver]"));
            RuleFor(sh => sh.Originator.DateOfScrutiny).LessThanOrEqualTo(sh => sh.Reviewer.DateOfScrutiny)
                                                       .WithMessage($"[Rev. {revCode}] Invalid date of scrutiny for Originator. Date must be earlier or same as that of reviewer")
                                                       .LessThanOrEqualTo(sh => sh.Approver.DateOfScrutiny)
                                                       .WithMessage($"[Rev. {revCode}] Invalid date of scrutiny for Originator. Date must be earlier or same as that of approver");
            RuleFor(sh => sh.Reviewer.DateOfScrutiny).GreaterThanOrEqualTo(sh => sh.Originator.DateOfScrutiny)
                                                     .WithMessage($"[Rev. {revCode}] Invalid date of scrutiny for Reviewer. Date must be later or same as that of the originator")
                                                     .LessThanOrEqualTo(sh => sh.Approver.DateOfScrutiny)
                                                     .WithMessage($"[Rev. {revCode}] Invalid date of scrutiny for Reviewer. Date must be earlier or same as that of the approver");

            RuleFor(sh => sh.Approver.DateOfScrutiny).GreaterThanOrEqualTo(sh => sh.Originator.DateOfScrutiny)
                                                     .WithMessage($"[Rev. {revCode}] Invalid date of scrutiny for Approver. Date must be later or same as that of scrunity of originator")
                                                     .GreaterThanOrEqualTo(sh => sh.Reviewer.DateOfScrutiny)
                                                     .WithMessage($"[Rev. {revCode}] Invalid date of scrutiny for Approver. Date must be later or same as that of scrunity of reviewer");
        }
    }

    public class ScrutinizerValidator : AbstractValidator<IScrutinizer>
    {
        public ScrutinizerValidator(string revDescription)
        {
            RuleFor(s => s.ShortName).NotEmpty($"{revDescription} short name");
            RuleFor(s => s.FullName).NotEmpty($"{revDescription} full name");
            RuleFor(s => s.DateOfScrutiny).Must(d => d <= DateTime.Today).WithMessage($"Invalid date of scrutiny for {revDescription}. The date must not be a future date.");
        }
    }

    public class InputValidator : AbstractValidator<IInput>
    {
        public InputValidator()
        {
            RuleFor(i => i.BridgeDefinition).NotNull("Bridge Definition").SetValidator(new BridgeDefinitionValidator());
            RuleFor(i => i.LiquidContainersSeismicDefinition).NotNull("Liquid Containers Seismic Definition").SetValidator(new LiquidContainersSeismicDefinitionValidator());
        }
    }

    public class SettingsValidator : AbstractValidator<ISettings>
    {
        public SettingsValidator()
        {
            RuleFor(s => s.ProjectData).NotNull("Project Data").SetValidator(new ProjectDataValidator());

        }
    }

    public class BridgeDefinitionValidator : AbstractValidator<IBridgeDefinition>
    {
        public BridgeDefinitionValidator()
        {

        }
    }

    public class LiquidContainersSeismicDefinitionValidator : AbstractValidator<ILiquidContainersSeismicDefinition>
    {
        public LiquidContainersSeismicDefinitionValidator()
        {

        }
    }

}
