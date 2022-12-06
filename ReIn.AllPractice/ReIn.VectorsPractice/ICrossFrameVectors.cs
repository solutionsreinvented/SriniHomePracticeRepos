using System.Windows.Media.Media3D;

namespace ReIn.VectorsPractice
{
    public interface ICrossFrameVectors
    {
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from A to B.
        /// </summary>
        Vector3D VectorAB { get; }
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from C to D.
        /// </summary>
        Vector3D VectorCD { get; }
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from A to C.
        /// </summary>
        Vector3D VectorAC { get; }
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from B to D.
        /// </summary>
        Vector3D VectorBD { get; }
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from B to A.
        /// </summary>
        Vector3D VectorBA { get; }
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from D to C.
        /// </summary>
        Vector3D VectorDC { get; }
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from C to A.
        /// </summary>
        Vector3D VectorCA { get; }
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from D to B.
        /// </summary>
        Vector3D VectorDB { get; }
    }
}