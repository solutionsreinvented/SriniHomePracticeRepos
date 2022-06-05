using WpfMvvmNavigation.Models;

namespace WpfMvvmNavigation.ViewModels
{
    public abstract class ViewModelBase : NotifyPropertyChanged
    {
        public string Title { get; set; }
    }
}
