using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using ReInvented.Shared.Base;
using ReInvented.Shared.Commands;

namespace ExternalAssembliesTester.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        public MainViewModel()
        {
            Items = new ObservableCollection<string>();
            AddItemCommand = new RelayCommand(OnAddItem, true);
        }

        private void OnAddItem()
        {
            int count = Items.Count;
            Items.Add($"New Item {count + 1}");
        }

        public ObservableCollection<string> Items { get; set; }

        public ICommand AddItemCommand { get; set; }
    }
}
