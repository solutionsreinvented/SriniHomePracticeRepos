using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomControlsTestProject.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _password;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {

        }


        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
