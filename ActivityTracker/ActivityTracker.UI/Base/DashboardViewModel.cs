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
using ActivityTracker.UI.ViewModels;

namespace ActivityTracker.UI.Base
{
    public abstract class DashboardViewModel : ViewModelBase
    {
        #region Parameterized Constructor
        public DashboardViewModel(NavigationStore navigationStore) : base(navigationStore)
        {

        } 
        #endregion

        #region Data Masters

        public ProjectMaster ProjectMaster { get => Get<ProjectMaster>(); protected set => Set(value); }

        public ActivityMaster ActivityMaster { get => Get<ActivityMaster>(); protected set => Set(value); }

        #endregion


        public IResource Resource { get; set; }

        #region DataGrid Source Providers

        public IEnumerable<IProject> PreOrders => ProjectMaster?.Projects?.Where(p => p.Type == ProjectType.PreOrder);// && p.Activities.Any(a => a.AllocatedResources.Contains(Resource)));

        public IEnumerable<IProject> Orders => ProjectMaster?.Projects?.Where(p => p.Type == ProjectType.Order);

        public IEnumerable<IProject> Developments => ProjectMaster?.Projects?.Where(p => p.Type == ProjectType.Development);

        public IEnumerable<IActivity> Activities => SelectedProject?.Activities;

        #endregion

        #region Public Properties
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

        public IActivity SelectedActivity { get => Get<IActivity>(); set { Set(value); ActivityIsSelected = value != null; } }
        #endregion

        #region Readonly Properties
        public string Title { get => Get<string>(); protected set => Set(value); }

        public bool UserIsAdmin { get => Get<bool>(); protected set => Set(value); }

        public bool ProjectIsSelected { get => Get<bool>(); private set => Set(value); }

        public bool ActivityIsSelected { get => Get<bool>(); private set => Set(value); }

        public bool ProjectsLoaded { get => Get<bool>(); private set => Set(value); }

        #endregion

        #region Commands
        public ICommand LoadProjectsMasterCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand SaveProjectsMasterCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand CreateActivityCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion

        #region Command Handlers
        private void OnSaveProjectsMaster()
        {
            ProjectMasterService.SaveToFile(ProjectMaster);
            _ = MessageBox.Show("Project master is saved successfully!", "Save project master", MessageBoxButton.OK);
        }

        private void OnLoadProjectsMaster()
        {
            ProjectMaster = ProjectMasterService.ReadFromFile();///.Retrieve();
            ProjectsLoaded = true;
            RaiseMultiplePropertiesChanged(nameof(PreOrders), nameof(Orders), nameof(Developments));
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
        #endregion

        #region Private Helpers

        protected virtual void InitializeProperties()
        {
            ProjectsLoaded = false;
            ActivityMaster = ActivityMasterService.ReadFromFile();

            LoadProjectsMasterCommand = new RelayCommand(OnLoadProjectsMaster, true);
            SaveProjectsMasterCommand = new RelayCommand(OnSaveProjectsMaster, true);
            CreateActivityCommand = new RelayCommand(OnCreateActivity, true);
        }


        #endregion

    }
}
