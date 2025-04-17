using System.Collections.Generic;

using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreGeometryExtensionsParallel
    {
        #region Fluent Functions

        #region Create

        public static OSCoreGeometry CreateMultipleNodes(this OSCoreGeometry geometry, HashSet<Node> nodes, int nThreads)
        {
            geometry.ComObject.CreateMultipleNodes(nodes: nodes, nThreads: nThreads);
            return geometry;
        }

        public static OSCoreGeometry CreateMultipleBeams(this OSCoreGeometry geometry, HashSet<Beam> beams, int nThreads)
        {
            geometry.ComObject.CreateMultipleBeams(beams: beams, nThreads: nThreads);
            return geometry;
        }

        public static OSCoreGeometry CreateMultiplePlates(this OSCoreGeometry geometry, HashSet<Plate> plates, int nThreads)
        {
            geometry.ComObject.CreateMultiplePlates(plates: plates, nThreads: nThreads);
            return geometry;
        } 

        #endregion

        #region Delete

        public static OSCoreGeometry DeleteExistingGeometry(this OSCoreGeometry geometry, int nThreads = 1)
        {
            geometry.DeleteAllSolids(nThreads);
            geometry.DeleteAllPlates(nThreads);
            geometry.DeleteAllBeams(nThreads);
            geometry.DeleteAllNodes(nThreads);

            return geometry;
        }

        public static OSCoreGeometry DeleteAllSolids(this OSCoreGeometry geometry, int nThreads)
        {
            geometry.ComObject.DeleteAllSolids(nThreads);
            return geometry;
        }


        public static OSCoreGeometry DeleteAllPlates(this OSCoreGeometry geometry, int nThreads)
        {
            geometry.ComObject.DeleteAllPlates(nThreads);
            return geometry;
        }

        public static OSCoreGeometry DeleteAllBeams(this OSCoreGeometry geometry, int nThreads)
        {
            geometry.ComObject.DeleteAllBeams(nThreads);
            return geometry;
        }

        public static OSCoreGeometry DeleteAllNodes(this OSCoreGeometry geometry, int nThreads)
        {
            geometry.ComObject.DeleteAllNodes(nThreads);
            return geometry;
        } 

        #endregion

        #endregion

        #region Get

        public static IEnumerable<int> GetIdsOfEntitiesInGroups(this OSCoreGeometry geometry, IEnumerable<string> groupNames, int nThreads)
        {
            return geometry.ComObject.GetIdsOfEntitiesInGroups(groupNames, nThreads);
        }

        public static HashSet<T> GetAllEntities<T>(this OSCoreGeometry geometry, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetAllEntities<T>(nThreads);
        }

        public static HashSet<T> GetEntities<T>(this OSCoreGeometry geometry, IEnumerable<int> entitiesIds, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntities<T>(entitiesIds, nThreads);
        }

        public static HashSet<EntityGroup<T>> GetEntityGroups<T>(this OSCoreGeometry geometry, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntityGroups<T>(nThreads);
        }

        public static HashSet<EntityGroup> GetEntityGroups<T>(this OSCoreGeometry geometry, string groupNameContaining, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntityGroups<T>(groupNameContaining, nThreads);
        }


        public static HashSet<EntityGroup<T>> GetEntityGroups<T>(this OSCoreGeometry geometry, HashSet<T> allEntities, int nThreads) where T : IEntity
        {
            return geometry.ComObject.GetEntityGroups<T>(allEntities, nThreads);
        }

        public static HashSet<T> GetEntitiesInGroups<T>(this OSCoreGeometry geometry, IEnumerable<string> groupNames, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntitiesInGroups<T>(groupNames, nThreads);
        }

        public static HashSet<T> GetEntitiesInGroup<T>(this OSCoreGeometry geometry, string groupName, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntitiesInGroup<T>(groupName, nThreads);
        }

        public static HashSet<T> GetEntitiesInGroup<T>(this OSCoreGeometry geometry, string groupName, HashSet<T> allEntities, int nThreads) where T : IEntity
        {
            return geometry.ComObject.GetEntitiesInGroup<T>(groupName, allEntities, nThreads);
        }

        public static IEnumerable<Node> GetUniqueNodesFromPlateGroup(this OSCoreGeometry geometry, string platesGroupName, int nThreads)
        {
            return geometry.ComObject.GetUniqueNodesFromPlateGroup(platesGroupName, nThreads);
        }

        /// Newly added functions

        public static IEnumerable<Beam> GetBeamsWithNoGroupAssignment(this OSCoreGeometry geometry, int nThreads)
        {
            return geometry.ComObject.GetBeamsWithNoGroupAssignment(nThreads);
        }

        public static IEnumerable<Plate> GetPlatesWithNoGroupAssignment(this OSCoreGeometry geometry, int nThreads)
        {
            return geometry.ComObject.GetPlatesWithNoGroupAssignment(nThreads);
        } 

        #endregion
    }
}
