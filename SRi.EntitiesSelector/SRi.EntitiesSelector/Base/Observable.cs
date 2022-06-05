using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SRi.EntitiesSelector.Base
{
    public abstract class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
