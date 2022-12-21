using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PerformanceManager.Domain.Base
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected void RaiseMultiplePropertiesChanged([CallerMemberName] string propertyName = "")
        {
            RaisePropertyChanged(propertyName);
        }

        protected void RaiseAllChanged()
        {
            RaisePropertyChanged("");
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaiseMultiplePropertiesChanged(params string[] properties)
        {
            properties.ToList().ForEach(propertyName => RaisePropertyChanged(propertyName));
        }
    }
}
