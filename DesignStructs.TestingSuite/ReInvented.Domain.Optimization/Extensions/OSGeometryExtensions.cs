using System.Collections.Generic;
using System.Threading.Tasks;

using OpenSTAADUI;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;

namespace ReInvented.Domain.Optimization.Extensions
{
    public static class OSGeometryExtensions
    {
        public static List<(int NodeId, double Fx, double Fz)> DistributeTorque(this OSGeometryUI geometry, string nodeGroupName, double torqueToBeDistributed, int nThreads)
        {
            HashSet<Node> nodes = geometry.GetEntitiesInGroup<Node>(nodeGroupName, nThreads);
            Node center = nodes.GetCenter();

            return nodes.DistributeTorque(center, torqueToBeDistributed);
        }

        public async static Task<List<(int NodeId, double Fx, double Fz)>> DistributeTorqueAsync(this OSGeometryUI geometry, string nodeGroupName, double torqueToBeDistributed, int nThreads)
        {
            HashSet<Node> nodes = await geometry.GetEntitiesInGroupAsync<Node>(nodeGroupName, nThreads);
            Node center = nodes.GetCenter();

            return nodes.DistributeTorque(center, torqueToBeDistributed);
        }
    }
}
