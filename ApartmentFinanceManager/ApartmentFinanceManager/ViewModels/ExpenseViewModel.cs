using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

using ReInvented.Shared.Commands;

using System;
using System.Linq;
using System.Windows.Input;

namespace ApartmentFinanceManager.ViewModels
{
    public class ExpenseViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _sender;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private ExpenseViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public ExpenseViewModel(BaseViewModel sender, NavigationService navigationService, Flat flatToBeProcessed)
            : this()
        {
            _sender = (SummaryViewModel)sender;
            _navigationService = navigationService;
            FlatToBeProcessed = flatToBeProcessed;
        }

        #endregion

        #region Public Properties

        public Expense Expense { get => Get<Expense>(); set => Set(value); }

        public Flat FlatToBeProcessed { get => Get<Flat>(); set => Set(value); }

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
            Flat? senderFlat = _sender.ApartmentBlock.Flats.FirstOrDefault(f => f.Description == FlatToBeProcessed.Description);

            if (!senderFlat.HasAMatchingExistingExpense(Expense))
            {
                senderFlat.AddExpense(Expense);
            }

            _navigationService.CurrentViewModel = _sender;

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
