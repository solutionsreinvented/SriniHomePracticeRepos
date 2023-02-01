using Newtonsoft.Json;

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

        [JsonProperty]
        public PenaltyCriteria PenaltyCriteria { get => Get<PenaltyCriteria>(); private set => Set(value); }

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

            PenaltyCriteria = new PenaltyCriteria()
            {
                CommencesFrom = new DateTime(2022, 10, 01),
                Percentage = 20.0m,
                ReceiptCutoffDay = 01,
                MinimumOutstanding = 5600.0m
            };

            PenaltyCriteria.PenaltyCriteriaChanged -= OnPenaltyCriteriaChanged;
            PenaltyCriteria.PenaltyCriteriaChanged += OnPenaltyCriteriaChanged;

            ConsiderPenalties = true;
        }

        private void OnPenaltyCriteriaChanged(PenaltyCriteria sender, EventArgs e)
        {
            UpdateFlatPenalties();
        }

        private void UpdateFlatPenalties()
        {
            if (ConsiderPenalties)
            {
                Flats?.ForEach(f => f.GeneratePenalties(PenaltyCriteria));
            }
            else
            {
                Flats?.ForEach(f => f.Penalties?.Clear());
            }
        }

        #endregion
    }
}
