using ReInvented.DialogManagement.Commands;
using ReInvented.DialogManagement.Interfaces;
using System.Windows.Input;

namespace ReInvented.DialogManagement.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly IDialogService _dialogService;

        public MainWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            SelectSectionCommand = new RelayCommand(OnSelectSection, true);
        }


        private void OnSelectSection()
        {
            SectionSelectionViewModel sectionVM = new SectionSelectionViewModel();
            bool? result = _dialogService.ShowDialog(sectionVM);

            if (result.HasValue)
            {
                if (result.Value)
                {

                }
                else
                {
                    
                }
            }
        }

        public ICommand SelectSectionCommand { get; }
    }
}
