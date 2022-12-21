using System;

using ReIn.VectorsPractice.Enums;

namespace ReIn.VectorsPractice.Interfaces
{
    public interface IFrameGrid : IReferenceGrid, ICrossFrameVectors, ILongitudinalFrameVectors
    {

        FrameGridType GridType { get; set; }
        /// <summary>
        /// Width of the bridge at the respective grid.
        /// </summary>
        double Width { get; set; }
        /// <summary>
        /// Height of the bridge at the respective grid.
        /// </summary>
        double Height { get; set; }
        /// <summary>
        /// Spacing of this frame grid relative to the previous frame grid. The relative spacing for the first grid shall always be zero.
        /// </summary>
        double Spacing { get; }
        /// <summary>
        /// Absolute distance of this frame grid from the reference grid i.e., start grid of the bridge.
        /// </summary>
        double Distance { get; set; }

    }
}
