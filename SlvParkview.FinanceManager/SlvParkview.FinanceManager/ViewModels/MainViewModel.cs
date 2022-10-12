using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;
using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class MainViewModel : PropertyStore
    {
        #region Private Fields

        private SummaryViewModel _summaryViewModel;
        private NavigationService _navigationService;

        #endregion

        #region Default Constructor

        public MainViewModel()
        {
            InitializeNavigationService();
            Initialize();
        }

        #endregion

        #region Public Properties

        public ThemeViewModel ThemeViewModel { get; set; }

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); set => Set(value); }

        public DataManagementService DataManagementService { get => Get<DataManagementService>(); private set => Set(value); }

        #endregion

        #region Event Handlers

        private void OnNavigationServiceCurrentViewModelChanged()
        {
            CurrentViewModel = _navigationService.CurrentViewModel;
        }

        #endregion

        #region Public Commands

        public ICommand SaveDataCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateDataCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand RetrieveDataCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        /// <summary>
        /// Retrieves the existing data from a Json file.
        /// </summary>
        private void OnRetrieveData()
        {
            Block deserializedData = DataManagementService.RetrieveData();

            _summaryViewModel = new SummaryViewModel(_navigationService) { Block = deserializedData };

            _navigationService.CurrentViewModel = _summaryViewModel;
        }

        private void OnGenerateData()
        {

        }

        /// <summary>
        /// Persists the contents of the entire Block.
        /// </summary>
        private void OnSaveData()
        {
            DataManagementService?.SaveData(_summaryViewModel.Block);
        }

        #endregion

        #region Private Helpers

        private void InitializeNavigationService()
        {
            _navigationService = new NavigationService();

            _navigationService.CurrentViewModelChanged -= OnNavigationServiceCurrentViewModelChanged;
            _navigationService.CurrentViewModelChanged += OnNavigationServiceCurrentViewModelChanged;
        }

        private void Initialize()
        {
            DataManagementService = DataManagementService.Instance;

            ThemeViewModel = new ThemeViewModel();

            _summaryViewModel = new SummaryViewModel(_navigationService);

            SaveDataCommand = new RelayCommand(OnSaveData, true);
            GenerateDataCommand = new RelayCommand(OnGenerateData, true);
            RetrieveDataCommand = new RelayCommand(OnRetrieveData, true);

            _navigationService.CurrentViewModel = _summaryViewModel;
        }

        #endregion

    }
}
