using ReInvented.Shared.Stores;

using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Models
{
    public class Apartment : PropertyStore
    {
        public string Name { get => Get<string>(); set => Set(value); }

        public List<Block> Blocks { get => Get<List<Block>>(); set => Set(value); }
    }
}
