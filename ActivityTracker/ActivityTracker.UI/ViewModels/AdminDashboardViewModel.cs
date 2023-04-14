using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Models;
using ActivityTracker.UI.Base;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.ViewModels
{
    public class AdminDashboardViewModel : DashboardViewModel
    {
        #region Parameterized Constructor
        public AdminDashboardViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            InitializeProperties();
        }
        #endregion



        #region Public Properties

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

        #endregion

        #region Commands

        public ICommand CreateProjectCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand DeleteSelectedProjectCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand DeleteSelectedActivityCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion


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

        protected override void InitializeProperties()
        {

            Title = "Admin Dashboard";
            UserIsAdmin = true;

            base.InitializeProperties();

            //ResourceRepository resourceRepository = new();
            //Resources = resourceRepository.GetAll();

            CreateProjectCommand = new RelayCommand(OnCreateProject, true);
            DeleteSelectedProjectCommand = new RelayCommand(OnDeleteSelectedProject, true);
            DeleteSelectedActivityCommand = new RelayCommand(OnDeleteSelectedActivity, true);
        }

        #endregion

    }
}
