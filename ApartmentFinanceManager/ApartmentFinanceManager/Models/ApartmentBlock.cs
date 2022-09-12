using ReInvented.Shared.Stores;
using System.Collections.Generic;

namespace ApartmentFinanceManager.Models
{
    public class ApartmentBlock : PropertyStore
    {
        public ApartmentBlock()
        {

        }

        public string Name { get; set; }

        public List<Flat> Flats { get; set; }
    }
}
