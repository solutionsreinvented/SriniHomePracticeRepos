using ReInvented.Shared.Stores;

using System;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Models
{
    public class Block : PropertyStore
    {
        #region Default Constructor

        public Block()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public DateTime LastUpdated { get => Get<DateTime>(); set => Set(value); }

        public string Name { get => Get<string>(); set => Set(value); }

        public List<Flat> Flats { get => Get<List<Flat>>(); set => Set(value); }
        /// <summary>
        /// Date from which the penalties for delay in maintenance receipts will be applicable.
        /// </summary>
        public DateTime PenaltyCommencesFrom { get => Get<DateTime>(); set { Set(value); UpdateFlatPenalties(); } }
        /// <summary>
        /// Percentage of outstanding due which is added as penalty.
        /// </summary>
        public decimal PenaltyPercentage { get => Get<decimal>(); set { Set(value); UpdateFlatPenalties(); } }
        /// <summary>
        /// Amount beyond which the penalty is applicable.
        /// </summary>
        public decimal MinimumOutstandingForPenalty { get => Get<decimal>(); set { Set(value); UpdateFlatPenalties(); } }
        /// <summary>
        /// Cutoff date by which the maintenance outstanding shall be cleared.
        /// </summary>
        public int ReceiptCutoffDay { get => Get<int>(); set { Set(value); UpdateFlatPenalties(); } }
        /// <summary>
        /// Indicates whether penalties to be considered in the calculations
        /// </summary>
        public bool ConsiderPenalties { get => Get<bool>(); set { Set(value); UpdateFlatPenalties(); } }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            InitializePenaltyStuff();
        }

        private void InitializePenaltyStuff()
        {
            PenaltyCommencesFrom = new DateTime(2022, 10, 01);
            PenaltyPercentage = 20.0m;
            ReceiptCutoffDay = 10;
            MinimumOutstandingForPenalty = 5600.0m;
            ConsiderPenalties = true;
        }

        private void UpdateFlatPenalties()
        {
            if (ConsiderPenalties)
            {
                Flats?.ForEach(f => f.GeneratePenalties(this));
            }
            else
            {
                Flats?.ForEach(f => f.Penalties.Clear());
            }
        }

        #endregion
    }
}
