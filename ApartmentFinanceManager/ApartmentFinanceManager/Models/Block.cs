using ReInvented.Shared.Stores;
using System.Collections.Generic;

namespace ApartmentFinanceManager.Models
{
    public class Block : PropertyStore
    {
        #region Default Constructor

        public Block()
        {

        }

        #endregion

        #region Public Properties

        public string Name { get => Get<string>(); set => Set(value); }

        public List<Flat> Flats { get => Get<List<Flat>>(); set => Set(value); }

        #endregion
    }
}
