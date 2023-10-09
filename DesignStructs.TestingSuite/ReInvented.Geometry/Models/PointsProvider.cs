using System.Collections.Generic;

using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
    public class PointsProvider
    {
        public static HashSet<Node> GetPoints()
        {
            return new HashSet<Node>()
            {
                new Node(1, 2.5, 3.5, 0),
                new Node(2, 0, 0, 0), 
                new Node(3, 0, 5, 0), 
                new Node(4, -5, 10, 0), 
                new Node(5, 0, 15, 0), 
                new Node(6, 0, 20, 0), 
                new Node(7, 5, 25, 0),
                new Node(8, 10, 20, 0), 
                new Node(9, 15, 17.5, 0), 
                new Node(10, 20, 12.5, 0), 
                new Node(11, 12.5, 7.5, 0),
                new Node(12, 10, 2.5, 0)
            };
        }
    }
}
