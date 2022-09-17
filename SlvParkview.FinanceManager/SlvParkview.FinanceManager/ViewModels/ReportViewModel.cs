
using ReInvented.Shared.Commands;

using System;
using System.Collections.Generic;
using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;
using System.IO;
using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;
using System.Windows;
using SlvParkview.FinanceManager.Reporting;
using SlvParkview.FinanceManager.Enums;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        #region Private Fields

        private string _reportTargetDirectory;
        private readonly SummaryViewModel _sender;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private ReportViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public ReportViewModel(SummaryViewModel sender, NavigationService navigationService, Flat flatToBeProcessed)
            : this()
        {
            _sender = sender;
            _navigationService = navigationService;

            FlatToBeProcessed = flatToBeProcessed;
        }

        #endregion

        #region Public Properties

        public ReportType ReportType { get => Get(ReportType.BlockOverview); set => Set(value); }

        public Flat FlatToBeProcessed { get => Get<Flat>(); set => Set(value); }

        public FlatTransactionsReport FlatTransactionsReport { get => Get<FlatTransactionsReport>(); set => Set(value); }

        public OverviewReport OverviewReport { get => Get<OverviewReport>(); set => Set(value); }

        public List<TransactionInfo> TransactionsSummary
        {
            get => Get<List<TransactionInfo>>();
            private set => Set(value);
        }

        public DateTime ReportTill { get => Get(DateTime.Today); set => Set(value); }

        public bool IncludeFlatInformation { get => Get(true); set => Set(value); }

        public bool ShowTransactionHistory { get => Get(true); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand AddExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnAddExpense()
        {
            ExpenseViewModel expenseViewModel = new ExpenseViewModel(_sender, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = expenseViewModel;
        }

        private void OnAddPayment()
        {
            PaymentViewModel paymentViewModel = new PaymentViewModel(_sender, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = paymentViewModel;
        }

        private void OnAddCommonExpense()
        {
            CommonExpenseViewModel commonExpenseViewModel = new CommonExpenseViewModel(_sender, _navigationService);
            _navigationService.CurrentViewModel = commonExpenseViewModel;
        }

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            AddExpenseCommand = new RelayCommand(OnAddExpense, true);
            AddPaymentCommand = new RelayCommand(OnAddPayment, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);
            GenerateCommand = new RelayCommand(OnGenerate, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        /// <summary>
        /// Creates the required directories to store the json and html files of the report(s).
        /// </summary>
        private void CreateRequiredDirectories()
        {
            /// Create if the Reports directory does not exists.
            if (!Directory.Exists(ServiceProvider.ReportsDirectory))
            {
                _ = Directory.CreateDirectory(ServiceProvider.ReportsDirectory);
            }

            /// Create if a separate directory for the selected flat does not exists.

            string flatDirectory = Path.Combine(ServiceProvider.ReportsDirectory, FlatToBeProcessed.Description);

            if (!Directory.Exists(flatDirectory))
            {
                _ = Directory.CreateDirectory(flatDirectory);
            }

            /// Create a directory with requested date if it does not exists.

            string todayDirectory = Path.Combine(flatDirectory, ReportTill.ToString("dd MMM yyyy"));

            if (!Directory.Exists(todayDirectory))
            {
                _ = Directory.CreateDirectory(todayDirectory);
            }
            _reportTargetDirectory = todayDirectory;
        }

        private void CreateHtmlFile(string fileName)
        {
            File.Copy(Path.Combine(ServiceProvider.ReportsDirectory, "outstandings-template.html"), Path.Combine(_reportTargetDirectory, fileName).Replace("json", "html"), true);
        }

        private void CreateJsonFile(string fileName, string serializedData)
        {
            File.WriteAllText(Path.Combine(_reportTargetDirectory, fileName), serializedData);
        }

        #endregion

        private void OnGenerate()
        {
            IDataSerializer<FlatTransactionsReport> dataSerializer = new JsonDataSerializer<FlatTransactionsReport>();
            string fileName = $"Transactions Summary ({ReportTill:dd MMM yyyy}).json";

            CreateRequiredDirectories();

            CreateHtmlFile(fileName);

            FlatTransactionsReport = ReportsGenerator.GetFlatTransactionsReport(FlatToBeProcessed, ReportTill);

            string serializedData = dataSerializer.Serialize(FlatTransactionsReport);

            CreateJsonFile(fileName, serializedData);

            _ = MessageBox.Show("Report generated successfully!", "Report Generation", MessageBoxButton.OK);

            //ReportViewerViewModel reportViewerViewModel = 
            //    new ReportViewerViewModel(_sender, _navigationService, Path.Combine(_reportTargetDirectory, fileName.Replace("json", "html")));
            //_navigationService.CurrentViewModel = reportViewerViewModel;
        }
    }
}
