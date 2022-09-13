using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

using System;
using System.IO;
using System.Reflection;
using System.Windows.Input;

namespace ApartmentFinanceManager.ViewModels
{
    public class MainViewModel : PropertyStore
    {
        private string _binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private string _filePath;

        #region Private Fields

        private NavigationService _navigationService;
        private IDataSerializer<ApartmentBlock> _dataSerializer;

        #endregion

        #region Default Constructor

        public MainViewModel()
        {
            InitializeNavigationService();
            Initialize();
        }

        #endregion

        #region Public Properties

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); set => Set(value); }

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
            _dataSerializer = new JsonDataSerializer<ApartmentBlock>();
            ApartmentBlock deserializedData = _dataSerializer.Deserialize(_filePath);

            SummaryViewModel summaryViewModel = new SummaryViewModel(_navigationService) { ApartmentBlock = deserializedData };

            _navigationService.CurrentViewModel = summaryViewModel;
        }

        private void OnGenerateData()
        {

        }

        private void OnSaveData()
        {
            _dataSerializer = new JsonDataSerializer<ApartmentBlock>();

            string serializedData = _dataSerializer.Serialize(((SummaryViewModel)CurrentViewModel).ApartmentBlock);

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
            _filePath = Path.Combine(_binDirectory, "C Block Data.json");

            BaseViewModel summaryViewModel = new SummaryViewModel(_navigationService);
            SaveDataCommand = new RelayCommand(OnSaveData, true);
            GenerateDataCommand = new RelayCommand(OnGenerateData, true);
            RetrieveDataCommand = new RelayCommand(OnRetrieveData, true);

            _navigationService.CurrentViewModel = summaryViewModel;
        }

        #endregion

    }
}
