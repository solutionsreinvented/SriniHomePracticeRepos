using System.Collections.Generic;
using System.Windows.Media.Media3D;

using ReInvented.StaadPro.Interop.Entities;

namespace ReInvented.Shared.Geometry.Interfaces
{
    public interface IPolygon
    {
        /// <summary>
        /// All points on the polygon including the closing points if generated.
        /// </summary>
        List<Node> Vertices { get; }
        /// <summary>
        /// Total included angle formed by the closed polygon.
        /// </summary>
        double TotalIncludeAngle { get; }

        Vector3DCollection Sides { get; }
    }
}
