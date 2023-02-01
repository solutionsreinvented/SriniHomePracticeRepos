using ReInvented.Shared.Stores;

using System;

namespace SlvParkview.FinanceManager.Models
{
    public class PenaltyCriteria : PropertyStore
    {
        public event Action<PenaltyCriteria, EventArgs> PenaltyCriteriaChanged;

        /// <summary>
        /// Date from which the penalties for delay in maintenance payments will be applicable.
        /// </summary>
        public DateTime CommencesFrom { get => Get<DateTime>(); set { Set(value); RaisePenaltyCriteriaChanged(); } }
        /// <summary>
        /// Percentage of outstanding due which is added as penalty.
        /// </summary>
        public decimal Percentage { get => Get<decimal>(); set { Set(value); RaisePenaltyCriteriaChanged(); } }
        /// <summary>
        /// Amount beyond which the penalty is applicable.
        /// </summary>
        public decimal MinimumOutstanding { get => Get<decimal>(); set { Set(value); RaisePenaltyCriteriaChanged(); } }
        /// <summary>
        /// Cutoff date by which the maintenance outstanding shall be cleared.
        /// </summary>
        public int ReceiptCutoffDay { get => Get<int>(); set { Set(value); RaisePenaltyCriteriaChanged(); } }

        private void RaisePenaltyCriteriaChanged()
        {
            PenaltyCriteriaChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
