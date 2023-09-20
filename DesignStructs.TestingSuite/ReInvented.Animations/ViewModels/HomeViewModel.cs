using System.Windows.Input;

using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace ReInvented.Animations.ViewModels
{
    public class HomeViewModel : ErrorsEnabledPropertyStore
    {
        public HomeViewModel()
        {
            CurrentViewModel = new RecentItemsViewModel();
            Initialize();
        }

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); private set => Set(value); }

        public ICommand RecentItemsCommand { get; set; }

        public ICommand NewProjectCommand { get; set; }


        private void Initialize()
        {
            RecentItemsCommand = new RelayCommand(OnRecentItems, true);
            NewProjectCommand = new RelayCommand(OnNewProject, true);
        }

        private void OnNewProject()
        {
            CurrentViewModel = new NewProjectViewModel();
        }

        private void OnRecentItems()
        {
            CurrentViewModel = new RecentItemsViewModel();
        }
    }
    public abstract class BaseViewModel : ErrorsEnabledPropertyStore
    {
        public BaseViewModel()
        {

        }

        public string Title { get => Get<string>(); set => Set(value); }
    }

    public class RecentItemsViewModel : BaseViewModel
    {
        public RecentItemsViewModel()
        {
            Title = "Recent Items";
        }
    }

    public class NewProjectViewModel : BaseViewModel
    {
        public NewProjectViewModel()
        {
            Title = "New Project";
        }
    }
}
