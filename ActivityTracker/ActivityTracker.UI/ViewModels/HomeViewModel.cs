using System.ComponentModel;
using System.Windows.Input;

using ActivityTracker.UI.Base;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Parameterized Constructor
        public HomeViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            LogoutCommand = new RelayCommand(OnLogout, true);
            _navigationStore.PropertyChanged += OnViewModelsChanged;
        }
        #endregion

        #region Content View Models
        public ViewModelBase DashboardViewModel => _navigationStore.DashboardViewModel;

        public ManageUserViewModel ManageUserViewModel => _navigationStore.ManageUserViewModel;
        #endregion

        #region Commands
        public ICommand MaximizeRestoreCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand MinimizeCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand CloseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand LogoutCommand { get => Get<ICommand>(); set => Set(value); }
        #endregion

        #region Command Handlers
        private void OnLogout()
        {
            _navigationStore.DashboardViewModel = null;
            _navigationStore.ManageUserViewModel = new LoginViewModel(_navigationStore);
        }
        #endregion

        #region Event Handlers
        private void OnViewModelsChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseMultiplePropertiesChanged(nameof(DashboardViewModel));
            RaiseMultiplePropertiesChanged(nameof(ManageUserViewModel));
        }
        #endregion
    }
}
