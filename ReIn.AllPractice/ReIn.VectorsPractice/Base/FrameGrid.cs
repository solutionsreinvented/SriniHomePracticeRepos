using ReIn.VectorsPractice.Interfaces;
using ReIn.VectorsPractice.Models;

namespace ReIn.VectorsPractice.Base
{
    /// <summary>
    /// Stores the data related to bridge frames at each grid i.e., cross frame location to generate the bridge geometry.
    /// This is the base class for all kinds of frames e.g., BridgeFrame, ExtensionFrame and PortalFrame etc.
    /// </summary>
    public abstract class FrameGrid : ReferenceGrid, IFrameGrid,
                                      IReferenceGrid, ICrossFrameVectors, ILongitudinalFrameVectors
    {
        public FrameGrid()
        {

        }

        public FrameGrid(int id)
        {
            Id = id;
        }

        ///public int Id { get => Get<int>(); private set => Set(value); }

        public double Width { get => Get<double>(); set => Set(value); }

        public double Height { get => Get<double>(); set => Set(value); }

        public double Spacing { get => Get<double>(); set => Set(value); }

        public double Distance { get => Get<double>(); set => Set(value); }

        public IReferenceGrid ReferenceGrid { get => Get<IReferenceGrid>(); set => Set(value); }

    }
}
