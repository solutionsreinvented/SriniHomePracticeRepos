﻿using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

using ReInvented.Shared.Commands;

using System;
using System.Windows.Input;

namespace ApartmentFinanceManager.ViewModels
{
    public class CommonExpenseViewModel : BaseViewModel
    {

        #region Private Fields

        private readonly BaseViewModel _sender;
        private readonly NavigationService _navigationService;

        #endregion

        private CommonExpenseViewModel()
        {
            Initialize();
        }

        public CommonExpenseViewModel(BaseViewModel sender,
                                      NavigationService navigationService,
                                      ApartmentBlock apartmentToBeProcessed)
            : this()
        {
            _sender = sender;
            _navigationService = navigationService;

            ApartmentBlockToBeProcessed = apartmentToBeProcessed;
        }


        #region Public Properties

        public Expense Expense { get => Get<Expense>(); set => Set(value); }

        public ApartmentBlock ApartmentBlockToBeProcessed { get => Get<ApartmentBlock>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand SaveExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand CancelCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnCancel()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        private void OnSaveExpense()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Expense = new Expense();
            SaveExpenseCommand = new RelayCommand(OnSaveExpense, true);
            CancelCommand = new RelayCommand(OnCancel, true);
        }

        #endregion

    }
}