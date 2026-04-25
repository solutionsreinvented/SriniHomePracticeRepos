using System.ComponentModel;
using System.Windows.Input;

using ProdActivity.UI.Base;
using ProdActivity.UI.Commands;
using ProdActivity.UI.Stores;

namespace ProdActivity.UI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Parameterized Constructor

        public HomeViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            LogoutCommand = new RelayCommand(OnLogout, true);
            AdminLoginCommand = new RelayCommand(OnAdminLogin, true);
            ToggleThemeCommand = new RelayCommand(OnToggleTheme, true);
            _navigationStore.PropertyChanged += OnViewModelsChanged;
        }

        #endregion

        #region Content View Models

        public ViewModelBase DashboardViewModel => _navigationStore.DashboardViewModel;

        public ManageUserViewModel ManageUserViewModel => _navigationStore.ManageUserViewModel;

        #endregion

        #region Commands

        public ICommand MaximizeRestoreCommand { get => Get<ICommand>(); internal set => Set(value); }

        public ICommand MinimizeCommand { get => Get<ICommand>(); internal set => Set(value); }

        public ICommand CloseCommand { get => Get<ICommand>(); internal set => Set(value); }

        public ICommand LogoutCommand { get => Get<ICommand>(); internal set => Set(value); }

        public ICommand AdminLoginCommand { get => Get<ICommand>(); internal set => Set(value); }

        public ICommand ToggleThemeCommand { get => Get<ICommand>(); internal set => Set(value); }

        #endregion

        #region Notifications

        public string NotificationTitle { get => Get<string>(); set => Set(value); }
        public string NotificationMessage { get => Get<string>(); set => Set(value); }
        public bool IsNotificationVisible { get => Get<bool>(); set => Set(value); }

        public async void ShowNotification(string title, string message)
        {
            NotificationTitle = title;
            NotificationMessage = message;
            IsNotificationVisible = true;
            await System.Threading.Tasks.Task.Delay(5000);
            IsNotificationVisible = false;
        }

        #endregion

        #region Command Handlers

        private void OnLogout()
        {
            _navigationStore.DashboardViewModel = null;
            _navigationStore.ManageUserViewModel = new LoginViewModel(_navigationStore);
        }

        private void OnAdminLogin()
        {
            _navigationStore.DashboardViewModel = null;
            _navigationStore.ManageUserViewModel = new LoginViewModel(_navigationStore);
        }

        private void OnToggleTheme()
        {
            Themes.ThemeManager.ToggleTheme();
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
