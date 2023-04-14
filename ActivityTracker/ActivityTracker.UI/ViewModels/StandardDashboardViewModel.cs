using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Models;
using ActivityTracker.Domain.Services;
using ActivityTracker.UI.Base;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.ViewModels
{
    public class StandardDashboardViewModel : ViewModelBase
    {
        public StandardDashboardViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            Title = "I am the dashboard!";
            ActivityMaster = ActivityMasterService.ReadFromFile();
        }

        public string Title { get => Get<string>(); set => Set(value); }

        public IResource Resource { get => Get<IResource>(); set => Set(value); }

        public string UserPassword { get => Get<string>(); set => Set(value); }

        public string TestTextBoxValue { get => Get<string>(); set => Set(value); }

        public ActivityMaster ActivityMaster { get => Get<ActivityMaster>(); set => Set(value); }

        //public PreOrder SelectedPreOrder { get => Get<PreOrder>(); set => Set(value); }

        //public Order SelectedOrder { get => Get<Order>(); set => Set(value); }

    }
}
