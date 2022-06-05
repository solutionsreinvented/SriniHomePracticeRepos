using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfMvvmNavigation.Models
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
