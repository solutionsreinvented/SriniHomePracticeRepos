using ReInvented.Shared.Store;
using System.Collections.Generic;

namespace ApartmentFinanceManager.Models
{
    public class Block : PropertyStore
    {
        public Block()
        {

        }

        public string Name { get; set; }

        public List<Flat> Flats { get; set; }
    }
}
