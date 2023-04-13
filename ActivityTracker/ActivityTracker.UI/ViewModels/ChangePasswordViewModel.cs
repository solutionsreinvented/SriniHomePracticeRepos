﻿using System;
using System.Windows.Input;

using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Repositories;
using ActivityTracker.UI.Base;
using ActivityTracker.UI.Commands;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.ViewModels
{
    public class ChangePasswordViewModel : ManageUserViewModel
    {
        #region Private Fields

        private readonly UserRepository _usersRepository = new();

        #endregion

        #region Parameterized Constructor
        public ChangePasswordViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            Initialize();
        }
        #endregion

        #region Public Properties
        public IUser User { get => Get<IUser>(); private set => Set(value); }

        public string UserId
        {
            get => Get<string>();
            set { Set(value); User = GetUser(value); RaisePropertyChanged(nameof(CanChangePassword)); }
        }

        public string CurrentPassword
        {
            get => Get<string>();
            set { Set(value); RaiseMultiplePropertiesChanged(nameof(CanChangePassword), nameof(CurrentPasswordMatches)); }
        }

        public string NewPassword
        {
            get => Get<string>();
            set { Set(value); RaisePropertyChanged(nameof(CanChangePassword)); }
        }

        public string ConfirmPassword
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(CanChangePassword));
            }
        }
        #endregion

        #region Commands
        public ICommand SubmitCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoBackCommand { get => Get<ICommand>(); set => Set(value); }
        #endregion

        #region Readonly Properties
        public bool CanChangePassword => UserExists && CurrentPasswordMatches && NewPasswordsMatch();

        private bool UserExists => User != null;

        public bool CurrentPasswordMatches => User.Password == CurrentPassword;
        #endregion

        #region Command Handlers
        private void OnPasswordChange()
        {
            throw new NotImplementedException();
        }

        private void OnGoBack()
        {
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore) { UserId = User.Id.ToString() };
        }
        #endregion

        #region Private Helpers
        private IUser GetUser(string userId)
        {
            _ = int.TryParse(userId, out int result);

            return _usersRepository.GetById(result);
        }

        private bool NewPasswordsMatch()
        {
            return !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword) && NewPassword == ConfirmPassword;
        }
        #endregion

        #region Abstract Methods Implementation
        protected override void Initialize()
        {
            GoBackCommand = new RelayCommand(OnGoBack, true);
            SubmitCommand = new RelayCommand(OnPasswordChange, true);
        }
        #endregion
    }
}
