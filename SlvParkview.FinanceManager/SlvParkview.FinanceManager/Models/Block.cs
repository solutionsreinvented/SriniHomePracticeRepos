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

        }

        #endregion

        #region Public Properties

        public DateTime LastUpdated { get => Get<DateTime>(); set => Set(value); }

        public string Name { get => Get<string>(); set => Set(value); }

        public List<Flat> Flats { get => Get<List<Flat>>(); set => Set(value); }

        #endregion
    }
}
