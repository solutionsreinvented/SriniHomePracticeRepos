using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;
using PerformanceManager.Domain.Repositories;
using PerformanceManager.Domain.Services;
using PerformanceManager.UI.Commands;
using PerformanceManager.UI.Stores;

using ReInvented.Shared.Interfaces;

namespace PerformanceManager.UI.ViewModels
{
    public class AdminDashboardViewModel : ViewModelBase
    {
        public AdminDashboardViewModel(NavigationStore navigationStore, IDialogService dialogService)
            : base(navigationStore, dialogService)
        {
            InitializeProperties();
        }

        private void InitializeProperties()
        {

            Title = "Admin Dashboard";

            ResourceRepository resourceRepository = new();
            Resources = resourceRepository.GetAll();

            ProjectMaster = ProjectMasterService.Retrieve();
            ActivityMaster = ActivityMasterService.ReadFromFile();

            CreateProjectCommand = new RelayCommand(OnCreateProject, true);
            CreateActivityCommand = new RelayCommand(OnCreateActivity, true);
            DeleteSelectedProjectCommand = new RelayCommand(OnDeleteSelectedProject, true);
        }

        #region Data Masters

        public ProjectMaster ProjectMaster { get => Get<ProjectMaster>(); private set => Set(value); }

        public ActivityMaster ActivityMaster { get => Get<ActivityMaster>(); private set => Set(value); }

        #endregion


        public IProject SelectedProject { get => Get<IProject>(); set { Set(value); RaisePropertyChanged(nameof(ProjectIsSelected)); } }

        #region DataGrid Source Providers

        public IEnumerable<IProject> PreOrders => ProjectMaster.Projects.Where(p => p.Type == ProjectType.PreOrder);

        public IEnumerable<IProject> Orders => ProjectMaster.Projects.Where(p => p.Type == ProjectType.Order);

        public IEnumerable<IProject> Developments => ProjectMaster.Projects.Where(p => p.Type == ProjectType.Development);

        #endregion



        public string Title { get => Get<string>(); set => Set(value); }


        public IEnumerable<IResource> Resources { get => Get<IEnumerable<IResource>>(); set => Set(value); }

        public IEnumerable<IActivity> Activities => SelectedResource?.Activities;

        //public ProjectType SelectedProjectType
        //{
        //    get => Get<ProjectType>();
        //    set => Set(value);
        //}

        //public Discipline SelectedActivityType
        //{
        //    get => Get<Discipline>();
        //    set => Set(value);
        //}


        public IResource SelectedResource
        {
            get => Get(Resources?.First());
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(Activities));
            }
        }

        public List<Project> Projects { get; set; }

        public IActivity SelectedActivity { get => Get<IActivity>(); set => Set(value); }

        //public DateTime StartDate { get => Get(DateTime.Now); set => Set(value); }

        //public DateTime EndDate { get => Get(DateTime.Now); set => Set(value); }

        #region Commands

        public ICommand CreateActivityCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand CreateProjectCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand DeleteSelectedProjectCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand CancelCreateActivityCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion

        public bool ProjectIsSelected => SelectedProject != null;

        //public bool CanAddActivity => CheckIfActivityCanBeAdded();

        #region Command Handlers

        //private bool CheckIfActivityCanBeAdded()
        //{
        //    if (ActivityDefinition.Discipline == Discipline.Detailing &&
        //        (SelectedProject.Type == ProjectType.PreOrder || SelectedProject.Type == ProjectType.Development))
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        private void OnCreateProject()
        {
            CreateProjectViewModel viewModel = new();

            bool? result = _dialogService.ShowDialog(viewModel);

            if (result.HasValue && result.Value)
            {
                ProjectMaster.Projects.Add(viewModel.ProjectDefinition.Project);
                RaiseMultiplePropertiesChanged(nameof(PreOrders), nameof(Orders), nameof(Developments));
            }
        }

        private void OnCreateActivity()
        {
            CreateActivityViewModel viewModel = new();

            bool? result = _dialogService.ShowDialog(viewModel);

            if (viewModel.ActivityDefinition.Discipline == Discipline.Detailing &&
                (SelectedProject.Type == ProjectType.PreOrder || SelectedProject.Type == ProjectType.Development))
            {
                throw new InvalidOperationException($"{viewModel.ActivityDefinition.Discipline} activity cannot be added to {SelectedProject.Type}");
            }

            if (!SelectedProject.Activities.Contains(viewModel.ActivityDefinition.Activity))
            {
                SelectedProject.Activities.Add(viewModel.ActivityDefinition.Activity);
            }
        }

        private void OnDeleteSelectedProject()
        {
            _ = ProjectMaster.Projects.Remove(SelectedProject);
            RaiseMultiplePropertiesChanged(nameof(PreOrders), nameof(Orders), nameof(Developments));
            SelectedProject = ProjectMaster?.Projects?.FirstOrDefault();
        }

        #endregion

    }
}
