using System.Collections.Generic;
using System.Windows.Media.Media3D;

using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Interfaces
{
    public interface IPolygon
    {
        List<Node> Points { get; }

        double TotalIncludeAngle { get; }

        Vector3DCollection Vectors { get; }
    }
}
