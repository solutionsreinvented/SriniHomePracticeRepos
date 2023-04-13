using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Models;
using ActivityTracker.Domain.Services;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.ViewModels
{
    public class AdminDashboardViewModel : ViewModelBase
    {
        public AdminDashboardViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            InitializeProperties();
        }

        #region Data Masters

        public ProjectMaster ProjectMaster { get => Get<ProjectMaster>(); private set => Set(value); }

        public ActivityMaster ActivityMaster { get => Get<ActivityMaster>(); private set => Set(value); }

        #endregion

        public IProject SelectedProject
        {
            get => Get<IProject>();
            set
            {
                Set(value);
                ProjectIsSelected = value != null;
                RaisePropertyChanged(nameof(Activities));
                SelectedActivity = Activities?.FirstOrDefault();
            }
        }

        #region DataGrid Source Providers

        public IEnumerable<IProject> PreOrders => ProjectMaster?.Projects?.Where(p => p.Type == ProjectType.PreOrder);

        public IEnumerable<IProject> Orders => ProjectMaster?.Projects?.Where(p => p.Type == ProjectType.Order);

        public IEnumerable<IProject> Developments => ProjectMaster?.Projects?.Where(p => p.Type == ProjectType.Development);

        public IEnumerable<IActivity> Activities => SelectedProject?.Activities;

        #endregion

        public string Title { get => Get<string>(); set => Set(value); }

        public IEnumerable<IResource> Resources { get => Get<IEnumerable<IResource>>(); set => Set(value); }


        public IResource SelectedResource
        {
            get => Get(Resources?.First());
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(Activities));
            }
        }

        public IActivity SelectedActivity { get => Get<IActivity>(); set { Set(value); ActivityIsSelected = value != null; } }

        #region Commands

        public ICommand CreateActivityCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand CreateProjectCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand DeleteSelectedProjectCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand DeleteSelectedActivityCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand LoadProjectsMasterCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand SaveProjectsMasterCommand { get => Get<ICommand>(); private set => Set(value); }


        #endregion

        public bool ProjectIsSelected { get => Get<bool>(); private set => Set(value); }

        public bool ActivityIsSelected { get => Get<bool>(); private set => Set(value); }


        #region Command Handlers

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
            CreateActivityViewModel viewModel = new(SelectedProject);

            bool? result = _dialogService.ShowDialog(viewModel);

            if (result.HasValue && result.Value)
            {
                if (!SelectedProject.Activities.Contains(viewModel.ActivityDefinition.Activity))
                {
                    SelectedProject.Activities.Add(viewModel.ActivityDefinition.Activity);
                }
            }
        }

        private void OnDeleteSelectedProject()
        {
            _ = ProjectMaster.Projects.Remove(SelectedProject);
            RaiseMultiplePropertiesChanged(nameof(PreOrders), nameof(Orders), nameof(Developments));
            SelectedProject = ProjectMaster?.Projects?.FirstOrDefault();
        }

        private void OnDeleteSelectedActivity()
        {
            _ = SelectedProject.Activities.Remove(SelectedActivity);
            RaiseMultiplePropertiesChanged(nameof(Activities));
            SelectedActivity = SelectedProject?.Activities.FirstOrDefault();
        }

        #endregion

        #region Private Helpers

        private void InitializeProperties()
        {

            Title = "Admin Dashboard";

            //ResourceRepository resourceRepository = new();
            //Resources = resourceRepository.GetAll();

            ActivityMaster = ActivityMasterService.ReadFromFile();

            CreateProjectCommand = new RelayCommand(OnCreateProject, true);
            CreateActivityCommand = new RelayCommand(OnCreateActivity, true);
            DeleteSelectedProjectCommand = new RelayCommand(OnDeleteSelectedProject, true);
            DeleteSelectedActivityCommand = new RelayCommand(OnDeleteSelectedActivity, true);

            LoadProjectsMasterCommand = new RelayCommand(OnLoadProjectsMaster, true);
            SaveProjectsMasterCommand = new RelayCommand(OnSaveProjectsMaster, true);
        }

        private void OnSaveProjectsMaster()
        {
            ProjectMasterService.SaveToFile(ProjectMaster);
            _ = MessageBox.Show("Project master is saved successfully!", "Save project master", MessageBoxButton.OK);
        }

        private void OnLoadProjectsMaster()
        {
            ProjectMaster = ProjectMasterService.ReadFromFile();///.Retrieve();
            RaiseMultiplePropertiesChanged(nameof(PreOrders), nameof(Orders), nameof(Developments));
        }

        #endregion

    }
}
