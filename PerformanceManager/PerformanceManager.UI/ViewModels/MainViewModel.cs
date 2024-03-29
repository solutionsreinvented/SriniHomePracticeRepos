﻿using System.ComponentModel;
using System.Windows.Input;

using PerformanceManager.UI.Commands;
using PerformanceManager.UI.Stores;

namespace PerformanceManager.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            CloseCommand = new RelayCommand(OnClose, true);

            _navigationStore.PropertyChanged += OnCurrentViewModelChanged;
        }

        private void OnClose()
        {

        }

        private void OnCurrentViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseMultiplePropertiesChanged(nameof(CurrentViewModel));
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand MaximizeRestoreCommand { get; set; }

        public ICommand MinimizeCommand { get; set; }

        public ICommand CloseCommand { get; set; }
    }
}
