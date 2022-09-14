using SlvParkview.FinanceManager.Models;

using System.Linq;

namespace SlvParkview.FinanceManager.Repositories
{
    public class FlatRepository
    {
        public static Flat Create(Block block, string ownedBy)
        {
            return new Flat(block, ownedBy);
        }

        public static void Delete(Block block, string description)
        {
            Flat targetFlat = block.Flats.FirstOrDefault(f => f.Description == description);

            if (targetFlat != null)
            {
                block.Flats.Remove(targetFlat);
            }
        }
    }
}
