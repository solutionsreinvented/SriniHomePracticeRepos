using System.Collections.Generic;
using System.Threading.Tasks;

using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSGeometryCoreExtensionsAsync
    {
        #region Get Geometry Entities

        public static async Task<IEnumerable<int>> GetAllSolidsListAsync(this OSGeometryCore geometry)
        {
            return await geometry.ComObject.GetAllSolidsListAsync();
        }

        public static async Task<IEnumerable<int>> GetAllPlatesListAsync(this OSGeometryCore geometry)
        {
            return await geometry.ComObject.GetAllPlatesListAsync();
        }

        public static async Task<IEnumerable<int>> GetAllBeamsListAsync(this OSGeometryCore geometry)
        {
            return await geometry.ComObject.GetAllBeamsListAsync();
        }

        public static async Task<IEnumerable<int>> GetAllNodesListAsync(this OSGeometryCore geometry)
        {
            return await geometry.ComObject.GetAllNodesListAsync();
        }

        public static async Task<IEnumerable<int>> GetSelectedPlatesListAsync(this OSGeometryCore geometry)
        {
            return await geometry.ComObject.GetSelectedPlatesListAsync();
        }

        public static async Task<IEnumerable<int>> GetSelectedBeamsListAsync(this OSGeometryCore geometry)
        {
            return await geometry.ComObject.GetSelectedBeamsListAsync();
        }

        public static async Task<IEnumerable<int>> GetSelectedNodesListAsync(this OSGeometryCore geometry)
        {
            return await geometry.ComObject.GetSelectedNodesListAsync();
        }

        public static async Task<HashSet<int>> GetSelectedEntitiesIdsAsync<T>(this OSGeometryCore geometry) where T : class, IEntity
        {
            return await geometry.ComObject.GetSelectedEntitiesIdsAsync<T>();
        }

        public static async Task<HashSet<int>> GetAllEntitiesIdsAsync<T>(this OSGeometryCore geometry) where T : class, IEntity
        {
            return await geometry.ComObject.GetAllEntitiesIdsAsync<T>();
        }

        public static async Task<HashSet<T>> GetSelectedEntitiesAsync<T>(this OSGeometryCore geometry, int nThreads) where T : class, IEntity
        {
            return await geometry.ComObject.GetSelectedEntitiesAsync<T>(nThreads);
        }

        public static async Task<HashSet<T>> GetAllEntitiesAsync<T>(this OSGeometryCore geometry, int nThreads) where T : class, IEntity
        {
            return await geometry.ComObject.GetAllEntitiesAsync<T>(nThreads);
        }

        public static async Task<HashSet<T>> GetEntitiesAsync<T>(this OSGeometryCore geometry, IEnumerable<int> entitiesIds, int nThreads) where T : class, IEntity
        {
            return await geometry.ComObject.GetEntitiesAsync<T>(entitiesIds, nThreads);
        }

        #endregion

        #region Entity Groups & Entities

        public static async Task<HashSet<T>> GetEntitiesInGroupsAsync<T>(this OSGeometryCore geometry, IEnumerable<string> groupNames, int nThreads) where T : class, IEntity
        {
            return await geometry.ComObject.GetEntitiesInGroupsAsync<T>(groupNames, nThreads);
        }

        public static async Task<HashSet<T>> GetEntitiesInGroupAsync<T>(this OSGeometryCore geometry, string groupName, int nThreads) where T : class, IEntity
        {
            return await geometry.ComObject.GetEntitiesInGroupAsync<T>(groupName, nThreads);
        }

        public static async Task<HashSet<T>> GetEntitiesInGroupAsync<T>(this OSGeometryCore geometry, string groupName, HashSet<T> allEntities, int nThreads) where T : IEntity
        {
            return await geometry.ComObject.GetEntitiesInGroupAsync<T>(groupName, allEntities, nThreads);
        }

        public static async Task<HashSet<EntityGroup<T>>> GetEntityGroupsAsync<T>(this OSGeometryCore geometry, int nThreads) where T : class, IEntity
        {
            return await geometry.ComObject.GetEntityGroupsAsync<T>(nThreads);
        }

        public static async Task<HashSet<EntityGroup>> GetEntityGroupsAsync<T>(this OSGeometryCore geometry, string groupNameContaining, int nThreads) where T : class, IEntity
        {
            return await geometry.ComObject.GetEntityGroupsAsync<T>(groupNameContaining, nThreads);
        }

        public static async Task<HashSet<EntityGroup<T>>> GetEntityGroupsAsync<T>(this OSGeometryCore geometry, HashSet<T> allEntities, int nThreads) where T : IEntity
        {
            return await geometry.ComObject.GetEntityGroupsAsync<T>(allEntities, nThreads);
        }

        #endregion
    }
}
