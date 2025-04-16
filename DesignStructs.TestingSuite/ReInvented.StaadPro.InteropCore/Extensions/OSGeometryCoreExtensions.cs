using System.Collections.Generic;

using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSGeometryCoreExtensions
    {
        #region Fluent Functions

        public static OSGeometryCore CreateGroupFrom(this OSGeometryCore geometry, IEnumerable<EntityGroup> entityGroups)
        {
            geometry.ComObject.CreateGroupFrom(entityGroups);
            return geometry;
        }

        public static OSGeometryCore CreateGroupFrom(this OSGeometryCore geometry, EntityGroup entityGroup)
        {
            geometry.ComObject.CreateGroupFrom(entityGroup);
            return geometry;
        }

        #endregion

        #region Public Functions

        public static IEnumerable<string> GetGroupNames(this OSGeometryCore geometry, GroupType groupType)
        {
            return geometry.ComObject.GetGroupNames(groupType);
        }

        public static IEnumerable<string> GetAllGroupNames(this OSGeometryCore geometry) => geometry.ComObject.GetAllGroupNames();

        public static IEnumerable<int> GetIdsOfEntitiesInGroup(this OSGeometryCore geometry, string groupName)
        {
            return geometry.ComObject.GetIdsOfEntitiesInGroup(groupName);
        }

        public static HashSet<Node> GetSelectedNodesExt(this OSGeometryCore geometry)
        {
            return geometry.ComObject.GetSelectedNodesExt();
        }

        public static IEnumerable<Node> GetPlateIncidence(this OSGeometryCore geometry, int plateNumber)
        {
            return geometry.ComObject.GetPlateIncidence(plateNumber);
        }

        public static Plate GetPlate(this OSGeometryCore geometry, int plateId) => geometry.ComObject.GetPlate(plateId);

        public static Beam GetBeam(this OSGeometryCore geometry, int beamNumber) => geometry.GetBeam(beamNumber);

        public static Node GetNode(this OSGeometryCore geometry, int nodeNumber) => geometry.GetNode(nodeNumber);

        #endregion

        #region Get Geometry Entities

        public static HashSet<int> GetAllEntitiesIds<T>(this OSGeometryCore geometry) where T : class, IEntity => geometry.ComObject.GetAllEntitiesIds<T>();

        public static IEnumerable<int> GetAllSolidsList(this OSGeometryCore geometry) => geometry.ComObject.GetAllSolidsList();

        public static IEnumerable<int> GetAllPlatesList(this OSGeometryCore geometry) => geometry.ComObject.GetAllPlatesList();

        public static IEnumerable<int> GetAllBeamsList(this OSGeometryCore geometry) => geometry.ComObject.GetAllBeamsList();

        public static IEnumerable<int> GetAllNodesList(this OSGeometryCore geometry) => geometry.ComObject.GetAllNodesList();

        #endregion
    }
}
