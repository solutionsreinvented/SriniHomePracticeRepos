
using System;
using System.Windows;
using System.Windows.Input;

using FluentValidation.Results;

using ReInvented.Domain.ProjectSetup.Enums;
using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Tass.Common.Interfaces;
using ReInvented.FluentValidationExercise.Validators;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;
using ReInvented.ThickenerModelGenerator.UI.Base;
using ReInvented.ThickenerModelGenerator.UI.Dialogs.ViewModels;
using ReInvented.ThickenerModelGenerator.UI.Models;

namespace DesignStructs.FluentValidationExercise.ViewModels
{
    public class MainViewModel : ValidatablePropertyStore
    {

        public MainViewModel()
        {
            Project = new Project();
            IProjectData projectData = Project.Settings.ProjectData;
            projectData.Name = "Coal Handling Project";
            projectData.Client = "Takraf India Pvt. Ltd.";
            projectData.Code = "24-4042";
            projectData.ProjectDirectory = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\35m";
            projectData.Structure = "35m Diameter High Rate Thickener";

            Project.Settings.ReportSettings.GenerateMTO = true;
            Project.Settings.ReportSettings.GenerateFoundationLoadData = true;


            //var mtoReport = Project.MaterialTakeoffReport as MTOReport;
            //mtoReport.Document.Number = "4042A0TR035CX001";
            //mtoReport.Document.Revisions.Add(new Revision());

            ReportViewModel = new FLDReportViewModel(Project, null) { IsStandAlone = true };
            ValidateDataCommand = new RelayCommand(OnValidateData, true);
        }

        private void OnValidateData()
        {
            ProjectValidator = new ProjectValidator();
            var subValidator = new ContingenciesValidator();
            try
            {
                //ValidationResult = subValidator.Validate((Project.MaterialTakeoffReport as MTOReport).Contingencies);

                ValidationResult = ProjectValidator.Validate(Project);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public ReportViewModel ReportViewModel { get => Get<ReportViewModel>(); private set => Set(value); }

        public ProjectValidator ProjectValidator { get => Get<ProjectValidator>(); private set => Set(value); }

        public IProject Project { get => Get<IProject>(); private set => Set(value); }

        public ValidationResult ValidationResult { get => Get<ValidationResult>(); set => Set(value); }

        public ICommand ValidateDataCommand { get; private set; }

    }
}
