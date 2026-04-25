using System;
using System.Windows.Input;
using ProdActivity.Domain.Base;
using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Stores;
using ProdActivity.UI.Commands;
using ReInvented.Shared.EventData;
using ReInvented.Shared.Interfaces;

namespace ProdActivity.UI.ViewModels
{
    public class AddUserViewModel : PropertyStore, IDialogRequestClose
    {
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public User NewUser { get => Get<User>(); set => Set(value); }
        
        public string ErrorMessage { get => Get<string>(); set => Set(value); }
        
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public ICommand SaveCommand { get => Get<ICommand>(); private set => Set(value); }
        public ICommand DiscardCommand { get => Get<ICommand>(); private set => Set(value); }

        public AddUserViewModel()
        {
            NewUser = new User 
            { 
                UserRole = UserRole.Moderator 
            };
            
            SaveCommand = new RelayCommand(OnSave, true);
            DiscardCommand = new RelayCommand(() => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)), true);
        }

        private void OnSave()
        {
            if (string.IsNullOrWhiteSpace(NewUser.EmployeeId))
            {
                ErrorMessage = "Employee ID is required.";
                RaisePropertyChanged(nameof(HasError));
                return;
            }
            if (string.IsNullOrWhiteSpace(NewUser.FullName))
            {
                ErrorMessage = "Full Name is required.";
                RaisePropertyChanged(nameof(HasError));
                return;
            }
            if (string.IsNullOrWhiteSpace(NewUser.Password))
            {
                ErrorMessage = "Password is required.";
                RaisePropertyChanged(nameof(HasError));
                return;
            }

            ErrorMessage = string.Empty;
            RaisePropertyChanged(nameof(HasError));
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }
    }
}
