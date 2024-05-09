using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ReInvented.AsyncPracticeApp.Interfaces
{
    public interface IProgressViewModel : INotifyPropertyChanged
    {
        #region Public Properties

        ObservableCollection<IActionItem> Actions { get; set; }

        #endregion

        #region Public Functions

        IActionItem CreateNewActionItem(string caption, string description);

        void AddNewActionItem(string caption, string description);

        #endregion
    }
}
