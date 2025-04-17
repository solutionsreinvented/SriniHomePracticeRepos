using System.Collections.Generic;

using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreGeometryExtensions
    {
        #region Fluent Functions

        public static OSCoreGeometry CreateGroupFrom(this OSCoreGeometry geometry, IEnumerable<EntityGroup> entityGroups)
        {
            geometry.ComObject.CreateGroupFrom(entityGroups);
            return geometry;
        }

        public static OSCoreGeometry CreateGroupFrom(this OSCoreGeometry geometry, EntityGroup entityGroup)
        {
            geometry.ComObject.CreateGroupFrom(entityGroup);
            return geometry;
        }

        #endregion

        #region Public Functions

        public static IEnumerable<string> GetGroupNames(this OSCoreGeometry geometry, GroupType groupType)
        {
            return geometry.ComObject.GetGroupNames(groupType);
        }

        public static IEnumerable<string> GetAllGroupNames(this OSCoreGeometry geometry) => geometry.ComObject.GetAllGroupNames();

        public static IEnumerable<int> GetIdsOfEntitiesInGroup(this OSCoreGeometry geometry, string groupName)
        {
            return geometry.ComObject.GetIdsOfEntitiesInGroup(groupName);
        }

        public static HashSet<Node> GetSelectedNodesExt(this OSCoreGeometry geometry)
        {
            return geometry.ComObject.GetSelectedNodesExt();
        }

        public static IEnumerable<Node> GetPlateIncidence(this OSCoreGeometry geometry, int plateNumber)
        {
            return geometry.ComObject.GetPlateIncidence(plateNumber);
        }

        public static Plate GetPlate(this OSCoreGeometry geometry, int plateId) => geometry.ComObject.GetPlate(plateId);

        public static Beam GetBeam(this OSCoreGeometry geometry, int beamNumber) => geometry.GetBeam(beamNumber);

        public static Node GetNode(this OSCoreGeometry geometry, int nodeNumber) => geometry.GetNode(nodeNumber);

        #endregion

        #region Get Geometry Entities

        public static HashSet<int> GetAllEntitiesIds<T>(this OSCoreGeometry geometry) where T : class, IEntity => geometry.ComObject.GetAllEntitiesIds<T>();

        public static IEnumerable<int> GetAllSolidsList(this OSCoreGeometry geometry) => geometry.ComObject.GetAllSolidsList();

        public static IEnumerable<int> GetAllPlatesList(this OSCoreGeometry geometry) => geometry.ComObject.GetAllPlatesList();

        public static IEnumerable<int> GetAllBeamsList(this OSCoreGeometry geometry) => geometry.ComObject.GetAllBeamsList();

        public static IEnumerable<int> GetAllNodesList(this OSCoreGeometry geometry) => geometry.ComObject.GetAllNodesList();

        #endregion
    }
}
