using System.ComponentModel;

using ActivityTracker.UI.Base;

using ReInvented.Shared.Interfaces;

namespace ActivityTracker.UI.Stores
{
    public class NavigationStore
    {
        private DashboardViewModel _dashboardViewModel;
        private ManageUserViewModel _manageUserViewModel;


        public event PropertyChangedEventHandler PropertyChanged;

        public NavigationStore(IDialogService dialogService)
        {
            ///CurrentViewModel = new ChangePasswordViewModel(this);
            DialogService = dialogService;
        }

        public IDialogService DialogService { get; }

        public DashboardViewModel DashboardViewModel
        {
            get => _dashboardViewModel;
            set
            {
                if (_dashboardViewModel != value)
                {
                    _dashboardViewModel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DashboardViewModel)));
                }
            }
        }

        public ManageUserViewModel ManageUserViewModel
        {
            get => _manageUserViewModel;
            set
            {
                if (_manageUserViewModel != value)
                {
                    _manageUserViewModel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ManageUserViewModel)));
                }
            }
        }
    }
}
