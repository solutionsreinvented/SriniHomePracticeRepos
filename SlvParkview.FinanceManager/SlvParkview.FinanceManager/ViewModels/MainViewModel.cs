using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

using System.IO;
using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class MainViewModel : PropertyStore
    {

        #region Private Fields

        private string _filePath;
        private SummaryViewModel _summaryViewModel;
        private NavigationService _navigationService;
        private IDataSerializer<Block> _dataSerializer;

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

        #endregion

        #region Read-only Properties

        public bool AllowSave { get => Get<bool>(); private set => Set(value); }

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

        private void OnRetrieveData()
        {
            _dataSerializer = new JsonDataSerializer<Block>();
            Block deserializedData = _dataSerializer.Deserialize(_filePath);

            _summaryViewModel = new SummaryViewModel(_navigationService) { Block = deserializedData };

            _navigationService.CurrentViewModel = _summaryViewModel;

            AllowSave = true;
        }

        private void OnGenerateData()
        {

        }

        private void OnSaveData()
        {
            _dataSerializer = new JsonDataSerializer<Block>();

            ///string serializedData = _dataSerializer.Serialize(((SummaryViewModel)CurrentViewModel).Block);

            string serializedData = _dataSerializer.Serialize(_summaryViewModel.Block);

            File.WriteAllText(_filePath, serializedData);
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
            _filePath = Path.Combine(ServiceProvider.AppDirectory, "C Block Data.json");

            ThemeViewModel = new ThemeViewModel();
            ///BaseViewModel summaryViewModel = new SummaryViewModel(_navigationService);

            _summaryViewModel = new SummaryViewModel(_navigationService);


            AllowSave = false;

            SaveDataCommand = new RelayCommand(OnSaveData, true);
            GenerateDataCommand = new RelayCommand(OnGenerateData, true);
            RetrieveDataCommand = new RelayCommand(OnRetrieveData, true);

            _navigationService.CurrentViewModel = _summaryViewModel;
        }

        #endregion

    }
}
