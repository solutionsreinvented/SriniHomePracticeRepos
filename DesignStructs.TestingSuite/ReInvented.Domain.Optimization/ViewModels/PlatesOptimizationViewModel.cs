using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.Optimization.Models;
using ReInvented.Domain.Optimization.Services;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Interfaces;
using ReInvented.Shared.Services;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.Interop.Services;

namespace ReInvented.Domain.Optimization.ViewModels
{
    public class PlatesOptimizationViewModel : ValidatablePropertyStore
    {
        #region Default Constructor

        public PlatesOptimizationViewModel(IDialogService dialogService)
        {
            Initialize(dialogService);
        }

        #endregion

        #region Public Properties

        public IDialogService DialogService { get; private set; }

        public PlatesOptimizationReport Report { get => Get<PlatesOptimizationReport>(); private set => Set(value); }

        public OpenStaadWrapper Wrapper
        {
            get => Get<OpenStaadWrapper>();
            private set
            {
                Set(value);
                RaisePropertyChanged(nameof(IsModelSelected));
                RetrieveLoadCases();
            }
        }

        public bool IsModelSelected => Wrapper != null;

        public bool CanGenerateResults => IsDataValid();

        public bool CanSaveReport => AreResultsAvailable();

        public bool ShowProgress { get => Get<bool>(); private set => Set(value); }

        public string ProgressMessage { get => Get<string>(); private set => Set(value); }

        #endregion

        #region Commands

        public ICommand BrowseSourceStaadFileCommand { get; private set; }

        public ICommand GenerateResultsCommand { get; private set; }

        public ICommand SaveReportCommand { get; private set; }

        #endregion

        #region Command Handlers

        private void OnBrowseSourceStaadFile()
        {
            string filePath = FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Staad model files", "std"));
            Report.SourceStaadFile = filePath;
            Wrapper = OpenStaadWrapperProvider.Get(filePath);
        }

        private async void OnGenerateResults()
        {
            Stopwatch sw = new Stopwatch();

            PlatesOptimizationCriteria criteria = Report.Criteria;
            IEnumerable<ILoadCase> loadCases = criteria.LoadCasesRange.Where(lc => lc.Id >= criteria.StartLoadCase.Id && lc.Id <= criteria.EndLoadCase.Id);

            PlatesOptimizationService pos = new PlatesOptimizationService(Wrapper, criteria);

            sw.Start();

            try
            {
                ShowProgress = true;
                ProgressMessage = "Optimizing the plates....";
                Report.Results = await pos.OptimizeAllAsync(loadCases); ///pos.OptimizeAll(loadCases);

                MessageService.ShowMessage(DialogService, $"Completed optimization of plates in {TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)}", "Optimize Plates");
                RaisePropertyChanged(nameof(CanSaveReport));
            }
            catch (Exception ex)
            {
                ShowProgress = false;
                MessageService.ShowMessage(DialogService, $"Failed optimizing the plates. Check the message below for further details.{Environment.NewLine}{ex.Message}", "Optimize Plates");
            }

            sw.Stop();
        }

        private void OnSaveReport()
        {
            POReportDocumentsGenerationService rdgs = new POReportDocumentsGenerationService(Report, DialogService);
            rdgs.SaveReport();
        }

        #endregion

        #region Event Handlers

        private void OnCriteriaPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Wrapper == null)
            {
                return;
            }

            PlatesOptimizationCriteria criteria = sender as PlatesOptimizationCriteria;

            if (e.PropertyName == nameof(Report.Criteria.LoadCaseType))
            {
                criteria.LoadCasesRange = Wrapper.Load.GetAllLoadCasesOfType(criteria.LoadCaseType);
                criteria.StartLoadCase = criteria.LoadCasesRange.FirstOrDefault();
                criteria.EndLoadCase = criteria.LoadCasesRange.LastOrDefault();
            }

            RaiseMultiplePropertiesChanged(nameof(CanGenerateResults), nameof(CanSaveReport));
        }

        #endregion

        #region Private Helpers

        private void Initialize(IDialogService dialogService)
        {
            ShowProgress = false;
            Report = new PlatesOptimizationReport();
            DialogService = dialogService;

            BrowseSourceStaadFileCommand = new RelayCommand(OnBrowseSourceStaadFile, true);
            GenerateResultsCommand = new RelayCommand(OnGenerateResults, true);
            SaveReportCommand = new RelayCommand(OnSaveReport, true);

            Report.Criteria.PropertyChanged -= OnCriteriaPropertyChanged;
            Report.Criteria.PropertyChanged += OnCriteriaPropertyChanged;
        }

        private void RetrieveLoadCases()
        {
            if (Report != null && Report.Criteria != null)
            {
                IEnumerable<ILoadCase> loadCasesRange = Wrapper.Load.GetAllLoadCasesOfType(Report.Criteria.LoadCaseType);

                if (loadCasesRange != null && loadCasesRange.Count() >= 1)
                {
                    Report.Criteria.LoadCasesRange = loadCasesRange;
                    Report.Criteria.StartLoadCase = loadCasesRange.FirstOrDefault();
                    Report.Criteria.EndLoadCase = loadCasesRange.LastOrDefault();

                    RaiseMultiplePropertiesChanged(nameof(CanGenerateResults), nameof(CanSaveReport));
                }
            }
        }

        private bool IsDataValid()
        {
            return Report != null && Report.Criteria != null && Report.Criteria.LoadCasesRange != null &&
                   Report.Criteria.LoadCasesRange.Count() >= 1 && Report.Criteria.StartLoadCase != null &&
                   Report.Criteria.EndLoadCase != null &&
                   Report.Criteria.StartLoadCase.Id <= Report.Criteria.EndLoadCase.Id;
        }

        private bool AreResultsAvailable()
        {
            return Report != null && Report.Results != null && Report.Results.Count() >= 1;
        }

        #endregion
    }
}