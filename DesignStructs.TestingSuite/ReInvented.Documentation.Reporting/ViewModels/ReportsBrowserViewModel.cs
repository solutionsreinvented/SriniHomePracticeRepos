using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

using ReInvented.DataAccess.Services;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace ReInvented.Documentation.Reporting.ViewModels
{
    public class ReportsBrowserViewModel : BaseViewModel
    {
        private int _currentIndex;

        public ReportsBrowserViewModel()
        {
            HtmlFiles = new ObservableCollection<string>();

            string pagesDirectory = Path.Combine(FileServiceProvider.ApplicationDataDirectory, "Reports", "Templates", "Pages");

            Directory.GetFiles(pagesDirectory).ToList().ForEach(f => HtmlFiles.Add(f));

            _currentIndex = 0;
            CurrentFilename = HtmlFiles[_currentIndex];

            NextFileCommand = new RelayCommand(OnNextFile, true);
            PreviousFileCommand = new RelayCommand(OnPreviousFile, true);
        }

        private void OnNextFile()
        {
            if (_currentIndex + 1 >= HtmlFiles.Count)
            {
                _currentIndex = 0;
            }
            else
            {
                ++_currentIndex;
            }
            CurrentFilename = HtmlFiles[_currentIndex];
        }
        private void OnPreviousFile()
        {
            if (_currentIndex - 1 < 0)
            {
                _currentIndex = HtmlFiles.Count - 1;
            }
            else
            {
                --_currentIndex;
            }
            CurrentFilename = HtmlFiles[_currentIndex];
        }

        public ObservableCollection<string> HtmlFiles { get => Get<ObservableCollection<string>>(); private set => Set(value); }

        public string CurrentFilename { get => Get<string>(); private set => Set(value); }

        public ICommand NextFileCommand { get; set; }

        public ICommand PreviousFileCommand { get; set; }

    }
}
