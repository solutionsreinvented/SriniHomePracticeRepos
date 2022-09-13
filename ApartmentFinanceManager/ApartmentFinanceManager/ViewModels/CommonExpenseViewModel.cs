using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

using ReInvented.Shared.Commands;

using System;
using System.Windows;
using System.Windows.Input;

namespace ApartmentFinanceManager.ViewModels
{
    public class CommonExpenseViewModel : BaseViewModel
    {

        #region Private Fields

        private readonly SummaryViewModel _sender;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private CommonExpenseViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public CommonExpenseViewModel(SummaryViewModel sender, NavigationService navigationService) : this()
        {
            _sender = sender;
            _navigationService = navigationService;

            ApartmentBlockToBeProcessed = _sender.ApartmentBlock;
        }

        #endregion

        #region Public Properties

        public bool AllowDuplicateEntry { get => Get<bool>(); set => Set(value); }

        public Expense Expense { get => Get<Expense>(); set => Set(value); }

        public ApartmentBlock ApartmentBlockToBeProcessed { get => Get<ApartmentBlock>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand SaveExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnSaveExpense()
        {
            if (AllowDuplicateEntry)
            {
                _sender.ApartmentBlock.Flats.ForEach(f => f.AddExpense(Expense));
            }
            else
            {
                foreach (Flat flat in _sender.ApartmentBlock.Flats)
                {
                    if (!flat.ContainsSimilar(Expense))
                    {
                        flat.AddExpense(Expense);
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show($"It appears that the entry already exists for {flat.Description}. Do you still want to add this?", "Duplicate entry identified", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {
                            flat.AddExpense(Expense);
                        }
                    }
                }
            }

            Expense = new Expense();
        }

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Expense = new Expense();

            SaveExpenseCommand = new RelayCommand(OnSaveExpense, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        #endregion

    }
}