using System;
using System.Windows.Input;

using ReInvented.Domain.Optimization.Models;
using ReInvented.Shared.Commands;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Optimization.ViewModels
{
    public class PlatesOptimizationViewModel : ValidatablePropertyStore
    {
        public PlatesOptimizationViewModel()
        {
            Report = new PlatesOptimizationReport();
            GenerateResultsCommand = new RelayCommand(OnGenerateResults, true);
            SaveReportCommand = new RelayCommand(OnSaveReport, true);
        }

        #region Public Properties

        public PlatesOptimizationReport Report { get; set; }

        #endregion

        #region Commands

        public ICommand GenerateResultsCommand { get; private set; }

        public ICommand SaveReportCommand { get; private set; } 

        #endregion


        #region Command Handlers

        private void OnGenerateResults()
        {
            throw new NotImplementedException();
        }

        private void OnSaveReport()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
