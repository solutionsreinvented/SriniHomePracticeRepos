using System.Collections.Generic;

using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
    public class RegularPolygonPointsProvider
    {
        public static List<Node> GetPoints()
        {
            return new List<Node>()
            {
                new Node(1, -0.3, 0, -0.5),
                new Node(2, 0, 0, -0.5),
                new Node(3, 0, 0.2, -0.4),
                new Node(4, 0, 0.4, -0.3),
                new Node(5, 0, 0.6, -0.2),
                new Node(6, 0, 0.8, -0.1),
                new Node(7, 0, 1.0, 0),
                new Node(8, -0.3, 1.0, 0),
                new Node(9, -0.6, 1.0, 0),
                new Node(10, -0.9, 1.0, 0),
                new Node(11, -0.9, 0.8, -0.1)
            };
        }
    }

}
