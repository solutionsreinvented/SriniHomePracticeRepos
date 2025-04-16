using System.Collections.Generic;

using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSGeometryCoreExtensionsParallel
    {
        #region Fluent Functions

        #region Create

        public static OSGeometryCore CreateMultipleNodes(this OSGeometryCore geometry, HashSet<Node> nodes, int nThreads)
        {
            geometry.ComObject.CreateMultipleNodes(nodes: nodes, nThreads: nThreads);
            return geometry;
        }

        public static OSGeometryCore CreateMultipleBeams(this OSGeometryCore geometry, HashSet<Beam> beams, int nThreads)
        {
            geometry.ComObject.CreateMultipleBeams(beams: beams, nThreads: nThreads);
            return geometry;
        }

        public static OSGeometryCore CreateMultiplePlates(this OSGeometryCore geometry, HashSet<Plate> plates, int nThreads)
        {
            geometry.ComObject.CreateMultiplePlates(plates: plates, nThreads: nThreads);
            return geometry;
        } 

        #endregion

        #region Delete

        public static OSGeometryCore DeleteExistingGeometry(this OSGeometryCore geometry, int nThreads = 1)
        {
            geometry.DeleteAllSolids(nThreads);
            geometry.DeleteAllPlates(nThreads);
            geometry.DeleteAllBeams(nThreads);
            geometry.DeleteAllNodes(nThreads);

            return geometry;
        }

        public static OSGeometryCore DeleteAllSolids(this OSGeometryCore geometry, int nThreads)
        {
            geometry.ComObject.DeleteAllSolids(nThreads);
            return geometry;
        }


        public static OSGeometryCore DeleteAllPlates(this OSGeometryCore geometry, int nThreads)
        {
            geometry.ComObject.DeleteAllPlates(nThreads);
            return geometry;
        }

        public static OSGeometryCore DeleteAllBeams(this OSGeometryCore geometry, int nThreads)
        {
            geometry.ComObject.DeleteAllBeams(nThreads);
            return geometry;
        }

        public static OSGeometryCore DeleteAllNodes(this OSGeometryCore geometry, int nThreads)
        {
            geometry.ComObject.DeleteAllNodes(nThreads);
            return geometry;
        } 

        #endregion

        #endregion

        #region Get

        public static IEnumerable<int> GetIdsOfEntitiesInGroups(this OSGeometryCore geometry, IEnumerable<string> groupNames, int nThreads)
        {
            return geometry.ComObject.GetIdsOfEntitiesInGroups(groupNames, nThreads);
        }

        public static HashSet<T> GetAllEntities<T>(this OSGeometryCore geometry, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetAllEntities<T>(nThreads);
        }

        public static HashSet<T> GetEntities<T>(this OSGeometryCore geometry, IEnumerable<int> entitiesIds, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntities<T>(entitiesIds, nThreads);
        }

        public static HashSet<EntityGroup<T>> GetEntityGroups<T>(this OSGeometryCore geometry, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntityGroups<T>(nThreads);
        }

        public static HashSet<EntityGroup> GetEntityGroups<T>(this OSGeometryCore geometry, string groupNameContaining, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntityGroups<T>(groupNameContaining, nThreads);
        }


        public static HashSet<EntityGroup<T>> GetEntityGroups<T>(this OSGeometryCore geometry, HashSet<T> allEntities, int nThreads) where T : IEntity
        {
            return geometry.ComObject.GetEntityGroups<T>(allEntities, nThreads);
        }

        public static HashSet<T> GetEntitiesInGroups<T>(this OSGeometryCore geometry, IEnumerable<string> groupNames, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntitiesInGroups<T>(groupNames, nThreads);
        }

        public static HashSet<T> GetEntitiesInGroup<T>(this OSGeometryCore geometry, string groupName, int nThreads) where T : class, IEntity
        {
            return geometry.ComObject.GetEntitiesInGroup<T>(groupName, nThreads);
        }

        public static HashSet<T> GetEntitiesInGroup<T>(this OSGeometryCore geometry, string groupName, HashSet<T> allEntities, int nThreads) where T : IEntity
        {
            return geometry.ComObject.GetEntitiesInGroup<T>(groupName, allEntities, nThreads);
        }

        public static IEnumerable<Node> GetUniqueNodesFromPlateGroup(this OSGeometryCore geometry, string platesGroupName, int nThreads)
        {
            return geometry.ComObject.GetUniqueNodesFromPlateGroup(platesGroupName, nThreads);
        }

        /// Newly added functions

        public static IEnumerable<Beam> GetBeamsWithNoGroupAssignment(this OSGeometryCore geometry, int nThreads)
        {
            return geometry.ComObject.GetBeamsWithNoGroupAssignment(nThreads);
        }

        public static IEnumerable<Plate> GetPlatesWithNoGroupAssignment(this OSGeometryCore geometry, int nThreads)
        {
            return geometry.ComObject.GetPlatesWithNoGroupAssignment(nThreads);
        } 

        #endregion
    }
}
