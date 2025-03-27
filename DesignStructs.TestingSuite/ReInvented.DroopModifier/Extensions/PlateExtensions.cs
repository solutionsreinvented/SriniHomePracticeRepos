using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ReInvented.StaadPro.Interop.Entities;

namespace ReInvented.DroopModifier.Extensions
{
    public static class PlateExtensions
    {
        public static IEnumerable<Node> GetNodes(this Plate plate)
        {
            return new[] { plate.A, plate.B, plate.C, plate.D }.Where(node => node != null);
        }
    }
}
