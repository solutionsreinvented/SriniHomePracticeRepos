using System.ComponentModel;
using System.Windows;

using ReInvented.AsyncPracticeApp.Models;
using ReInvented.Shared.Stores;

namespace ReInvented.AsyncPracticeApp.Base
{
    public class BaseViewModel : ValidatablePropertyStore, INotifyPropertyChanged
    {
        public BaseViewModel(Application application, IViewProvider viewProvider)
        {
            Application = application;
            ViewProvider = viewProvider;
        }

        protected Application Application { get => Get<Application>(); private set => Set(value); }

        protected IViewProvider ViewProvider { get => Get<IViewProvider>(); private set => Set(value); }
    }
}
