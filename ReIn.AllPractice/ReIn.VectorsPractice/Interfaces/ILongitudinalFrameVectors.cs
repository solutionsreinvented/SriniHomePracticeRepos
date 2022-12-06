using System.Windows.Media.Media3D;

namespace ReIn.VectorsPractice.Interfaces
{
    public interface ILongitudinalFrameVectors
    {
        /// <summary>
        /// Provides a normalized <see cref="Vector3D"/> from point A pointing towards the origin.
        /// </summary>
        Vector3D VectorA { get; }
        /// <summary>
        /// Provides a normalized <see cref="Vector3D"/> from point B pointing towards the origin.
        /// </summary>
        Vector3D VectorB { get; }
        /// <summary>
        /// Provides a normalized <see cref="Vector3D"/> from point C pointing towards the origin.
        /// </summary>
        Vector3D VectorC { get; }
        /// <summary>
        /// Provides a normalized <see cref="Vector3D"/> from point D pointing towards the origin.
        /// </summary>
        Vector3D VectorD { get; }
    }
}