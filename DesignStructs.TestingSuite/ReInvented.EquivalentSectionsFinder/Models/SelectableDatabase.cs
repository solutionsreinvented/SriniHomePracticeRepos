using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

using ReInvented.Sections.Domain.Models;

namespace Continuum.EquivalentSectionsFinder.Models
{
    public class SelectableDatabase : Database, INotifyPropertyChanged
    {
        #region Private Fields

        private bool _isSelected;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Default Constructor

        public SelectableDatabase()
        {

        }

        #endregion

        #region Parameterized Constructor

        public SelectableDatabase(Database db)
        {
            foreach (PropertyInfo prop in typeof(Database).GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(this, prop.GetValue(db));
                }
            }
        }

        #endregion

        #region Public Properties

        public bool IsSelected { get => _isSelected; set { _isSelected = value; RaisePropertyChanged(); } }

        #endregion

        #region Event Handlers

        public void RaisePropertyChanged([CallerMemberName] string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        #endregion
    }
}
