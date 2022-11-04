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
        /// Date from which the penalties for delay in maintenance payments will be applicable.
        /// </summary>
        public DateTime PenaltyCommencesFrom { get => Get<DateTime>(); set => Set(value); }

        public decimal PenaltyPercentage { get => Get<decimal>(); set => Set(value); }

        public decimal MinimumOutstandingForPenalty { get => Get<decimal>(); set => Set(value); }

        /// <summary>
        /// Cutoff date by which the maintenance outstanding shall be cleared.
        /// </summary>
        public int PaymentCutoffDay { get => Get<int>(); set => Set(value); }


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
            PaymentCutoffDay = 10;
            MinimumOutstandingForPenalty = 5600.0m;
        }

        #endregion

    }
}
