using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;

namespace DevDrive.Extensions
{
    public static class OSGeometryExtensions
    {
        public static IEnumerable<Beam> GetBeamsWithNoGroupAssignment(this OSGeometryUI geometry, int nThreads)
        {
            HashSet<Beam> allBeams = geometry.GetAllEntities<Beam>(nThreads);
            HashSet<EntityGroup<Beam>> allBeamGroups = geometry.GetEntityGroups<Beam>(nThreads);

            return allBeams.Except(allBeamGroups.SelectMany(g => g.Entities));
        }

        public static IEnumerable<Plate> GetPlatesWithNoGroupAssignment(this OSGeometryUI geometry, int nThreads)
        {
            HashSet<Plate> allPlates = geometry.GetAllEntities<Plate>(nThreads);
            HashSet<EntityGroup<Plate>> platesWithGroups = geometry.GetEntityGroups<Plate>(nThreads);

            return allPlates.Except(platesWithGroups.SelectMany(g => g.Entities));
        }
    }
}
