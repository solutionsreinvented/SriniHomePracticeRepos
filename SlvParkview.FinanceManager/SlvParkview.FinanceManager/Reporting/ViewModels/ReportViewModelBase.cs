using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.ViewModels;

using System;
using System.ComponentModel;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    public abstract class ReportViewModelBase : PropertyStore
    {
        #region Private Fields

        private protected readonly SummaryViewModel _summaryViewModel;

        #endregion

        #region Parameterized Constructor

        public ReportViewModelBase(SummaryViewModel summaryViewModel)
        {
            _summaryViewModel = summaryViewModel;

            Initialize();
        }

        #endregion

        #region Public Properties

        public Block Block { get => Get<Block>(); set { Set(value); UpdateReport(); } }

        public DateTime ReportTill { get => Get<DateTime>(); set { Set(value); UpdateReport(); } }

        public IReport Report { get => Get<IReport>(); set => Set(value); }

        #endregion

        protected abstract void UpdateReport();


        #region Private Helpers

        private protected virtual void Initialize()
        {
            ReportTill = DateTime.Today;
            Block = _summaryViewModel.Block;

            Block.PenaltyCriteria.PenaltyCriteriaChanged -= OnPenaltyCriteriaChanged;
            Block.PenaltyCriteria.PenaltyCriteriaChanged += OnPenaltyCriteriaChanged;

            Block.PropertyChanged -= OnBlockPropertyChanged;
            Block.PropertyChanged += OnBlockPropertyChanged;
        }

        private void OnBlockPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Block.ConsiderPenalties))
            {
                UpdateReport();
            }
        }

        private void OnPenaltyCriteriaChanged(PenaltyCriteria criteria, EventArgs e)
        {
            UpdateReport();
        }

        #endregion

    }
}
