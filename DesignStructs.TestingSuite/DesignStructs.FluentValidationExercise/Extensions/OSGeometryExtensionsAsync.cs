using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenSTAADUI;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace DesignStructs.FluentValidationExercise.Extensions
{
    public static class OSGeometryExtensionsAsync
    {
        public static async Task<OSGeometryUI> CreateMultipleNodesAsync(this OSGeometryUI geometry, HashSet<Node> nodes)
        {
            double maxNodesPerBatch = 20000.0;

            int nTasks = (nodes.Count() / maxNodesPerBatch).Ceiling(1);
            for (int i = 0; i < nTasks; i++)
            {
                IEnumerable<Node> taskNodes = nodes.Skip(i * 20000).Take(20000);
                await Task.Run(() => CreateMultipleNodes(geometry, taskNodes.ToHashSet()));
            }

            return geometry;
        }

        public static OSGeometryUI CreateMultipleNodes(this OSGeometryUI geometry, HashSet<Node> nodes)
        {
            nodes.OrderBy(n => n.Id).ToList().ForEach(n => geometry.CreateNode(n.Id, n.X, n.Y, n.Z));
            return geometry;
        }
    }
}
