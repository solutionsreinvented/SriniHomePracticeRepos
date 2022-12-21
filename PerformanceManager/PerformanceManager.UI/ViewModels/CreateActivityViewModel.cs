using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;
using PerformanceManager.Domain.Repositories;
using PerformanceManager.UI.Stores;

namespace PerformanceManager.UI.ViewModels
{
    public class CreateActivityViewModel : ViewModelBase
    {
        public CreateActivityViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            InitializeProperties();
        }

        private void InitializeProperties()
        {
            Title = "Create Activity";

            ResourceRepository resourceRepository = new();

            Resources = resourceRepository.GetAll();
        }

        public string Title { get => Get<string>(); set => Set(value); }

        public IEnumerable<IResource> Resources { get => Get<IEnumerable<IResource>>(); set => Set(value); }

        public IEnumerable<IActivity> Activities => SelectedResource?.Activities;

        public ProjectType SelectedProjectType
        {
            get => Get<ProjectType>();
            set => Set(value);
        }

        public ActivityType SelectedActivityType
        {
            get => Get<ActivityType>();
            set => Set(value);
        }


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

        public DateTime StartDate { get => Get(DateTime.Now); set => Set(value); }

        public DateTime EndDate { get => Get(DateTime.Now); set => Set(value); }

        public ICommand CreateActivityCommand { get; set; }

        public ICommand CancelCreateActivityCommand { get; set; }
    }
}
