using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSTAADUI;
using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Helpers;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;

namespace ReInvented.Domain.Optimization.Extensions
{
    public static class OSOutputExtensionsAsync
    {
        public static async Task<IEnumerable<PlateCenterResults>> GetPlateCenterResultsAsync(this OSOutputUI output, IEnumerable<ILoadCase> loadCases, IEnumerable<Plate> plates, int nThreads)
        {
            ConcurrentBag<PlateCenterResults> results = new ConcurrentBag<PlateCenterResults>();
            await Task.Run(() => loadCases.AsParallel().WithDegreeOfParallelism(nThreads)
                                                       .ForAll(lc => plates.AsParallel().WithDegreeOfParallelism(nThreads)
                                                                                        .ForAll(p => results.Add(output.GetPlateCenterResults(lc, p)))));

            return results;
        }
    }
    public static class OSGlobalExtensionsAsync
    {
        public static async Task<IEnumerable<PlateCenterResults>> GetPlateCenterResultsForAllPlatesAsync(this OpenStaadWrapper wrapper, IEnumerable<ILoadCase> loadCases, int nThreads)
        {
            IEnumerable<Plate> allPlates = await wrapper.Geometry.GetAllEntitiesAsync<Plate>(nThreads);
            return await wrapper.Output.GetPlateCenterResultsAsync(loadCases, allPlates, nThreads);
        }
    }

    public static class OSGeometryExtensionsAsync
    {
        #region Get Geometry Entities

        public static async Task<int[]> GetAllSolidsListAsync(this OSGeometryUI geometry)
        {
            dynamic nSolids = await Task.Run(() => geometry.GetSolidCount());

            object solidsList = new int[nSolids];
            await Task.Run(() => geometry.GetSolidList(ref solidsList));

            return (int[])solidsList;
        }

        public static async Task<int[]> GetAllPlatesListAsync(this OSGeometryUI geometry)
        {
            dynamic nPlates = await Task.Run(() => geometry.GetPlateCount());

            object platesList = new int[nPlates];
            await Task.Run(() => geometry.GetPlateList(ref platesList));

            return (int[])platesList;
        }

        public static async Task<int[]> GetAllBeamsListAsync(this OSGeometryUI geometry)
        {
            dynamic nBeams = await Task.Run(() => geometry.GetMemberCount());

            object beamsList = new int[nBeams];
            await Task.Run(() => geometry.GetBeamList(ref beamsList));

            return (int[])beamsList;
        }

        public static async Task<int[]> GetAllNodesListAsync(this OSGeometryUI geometry)
        {
            dynamic nNodes = await Task.Run(() => geometry.GetNodeCount());

            object nodesList = new int[nNodes];
            await Task.Run(() => geometry.GetNodeList(ref nodesList));

            return (int[])nodesList;
        }

        #endregion



        public static async Task<HashSet<int>> GetAllEntitiesIdsAsync<T>(this OSGeometryUI geometry) where T : class, IEntity
        {
            IEnumerable<int> ids = new HashSet<int>();

            if (typeof(T) == typeof(Node))
            {
                ids = await GetAllNodesListAsync(geometry);
            }
            else if (typeof(T) == typeof(Beam))
            {
                ids = await GetAllBeamsListAsync(geometry);
            }
            else if (typeof(T) == typeof(Plate))
            {
                ids = await GetAllPlatesListAsync(geometry);
            }

            return ids.ToHashSet();
        }

        public static async Task<HashSet<T>> GetAllEntitiesAsync<T>(this OSGeometryUI geometry, int nThreads) where T : class, IEntity
        {
            IEnumerable<int> entitiesIds = await GetAllEntitiesIdsAsync<T>(geometry);
            return await GetEntitiesAsync<T>(geometry, entitiesIds, nThreads);
        }

        public static async Task<HashSet<T>> GetEntitiesAsync<T>(this OSGeometryUI geometry, IEnumerable<int> entitiesIds, int nThreads) where T : class, IEntity
        {
            ConcurrentBag<T> entities = new ConcurrentBag<T>();

            if (typeof(T) == typeof(Node))
            {
                await Task.Run(() => entitiesIds.AsParallel().WithDegreeOfParallelism(nThreads).ForAll(nId => entities.Add(geometry.GetNode(nId) as T)));
            }
            else if (typeof(T) == typeof(Beam))
            {
                await Task.Run(() => entitiesIds.AsParallel().WithDegreeOfParallelism(nThreads).ForAll(bId => entities.Add(geometry.GetBeam(bId) as T)));
            }
            else if (typeof(T) == typeof(Plate))
            {
                await Task.Run(() => entitiesIds.AsParallel().WithDegreeOfParallelism(nThreads).ForAll(pId => entities.Add(geometry.GetPlate(pId) as T)));
            }

            return entities.ToHashSet();
        }

        public static async Task<HashSet<EntityGroup<T>>> GetEntityGroupsAsync<T>(this OSGeometryUI geometry, int nThreads) where T : class, IEntity
        {
            HashSet<T> allEntities = await GetAllEntitiesAsync<T>(geometry, nThreads);
            return await GetEntityGroupsAsync(geometry, allEntities, nThreads);
        }

        public static async Task<HashSet<EntityGroup>> GetEntityGroupsAsync<T>(this OSGeometryUI geometry, string groupNameContaining, int nThreads) where T : class, IEntity
        {
            HashSet<EntityGroup<T>> entityGroups = await geometry.GetEntityGroupsAsync<T>(nThreads);
            return EntityGroup<T>.ToNonGeneric(entityGroups.Where(g => g.GroupName.Contains(groupNameContaining)).ToHashSet());
        }


        public static async Task<HashSet<EntityGroup<T>>> GetEntityGroupsAsync<T>(this OSGeometryUI geometry, HashSet<T> allEntities, int nThreads) where T : IEntity
        {
            GroupType groupType = GroupTypeHelpers.GetGroupType(typeof(T));

            ConcurrentBag<EntityGroup<T>> entityGroups = new ConcurrentBag<EntityGroup<T>>();

            int groupsCount = await Task.Run(() => geometry.GetGroupCount(groupType));
            object groupNames = new string[groupsCount];
            await Task.Run(() => geometry.GetGroupNames(groupType, ref groupNames));

            await Task.Run(() => ((string[])groupNames).AsParallel().WithDegreeOfParallelism(nThreads).ForAll(gn =>
            {
                EntityGroup<T> group = new EntityGroup<T>()
                {
                    GroupName = gn,
                    Entities = geometry.GetEntitiesInGroup(gn, allEntities, nThreads)
                };

                entityGroups.Add(group);
            }));

            return entityGroups.ToHashSet();
        }

    }
}
