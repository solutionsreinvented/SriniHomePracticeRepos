using System.Collections.Generic;
using System.Linq;

using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Helpers;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSGeometryCoreExtensions
    {
        #region Fluent Functions

        public static OSGeometryCore CreateGroupFrom(this OSGeometryCore geometry, IEnumerable<EntityGroup> entityGroups)
        {
            entityGroups.ToList().ForEach(eg => CreateGroupFrom(geometry, eg));
            return geometry;
        }

        public static OSGeometryCore CreateGroupFrom(this OSGeometryCore geometry, EntityGroup entityGroup)
        {
            geometry.ComObject.CreateGroupEx(GroupTypeHelpers.GetGroupType(entityGroup.EntityType), entityGroup.GroupName, entityGroup.Entities.Count(), entityGroup.Entities.ToArray());
            return geometry;
        }

        #endregion

        #region Public Functions

        public static IEnumerable<string> GetGroupNames(this OSGeometryCore geometry, GroupType groupType)
        {
            int groupCount = geometry.ComObject.GetGroupCount(groupType);
            object groupNames = new string[groupCount];

            geometry.ComObject.GetGroupNames(groupType, ref groupNames);

            return (string[])groupNames;
        }

        public static IEnumerable<string> GetAllGroupNames(this OSGeometryCore geometry)
        {
            List<string> groupNames = new List<string>();

            groupNames.AddRange(GetGroupNames(geometry, GroupType.Nodes));
            groupNames.AddRange(GetGroupNames(geometry, GroupType.Beams));
            groupNames.AddRange(GetGroupNames(geometry, GroupType.Plates));

            return groupNames;
        }

        public static IEnumerable<int> GetIdsOfEntitiesInGroup(this OSGeometryCore geometry, string groupName)
        {
            int entityCount = geometry.ComObject.GetGroupEntityCount(groupName);

            object entityNumbers = new int[entityCount];

            geometry.ComObject.GetGroupEntities(groupName, ref entityNumbers);

            return ((int[])entityNumbers).AsEnumerable();
        }

        public static HashSet<Node> GetSelectedNodesExt(this OSGeometryCore geometry)
        {
            dynamic count = geometry.ComObject.GetNoOfSelectedNodes();

            object nodeNumbers = new int[count];
            geometry.ComObject.GetSelectedNodes(ref nodeNumbers, 0);

            int[] nodeNumbersList = (int[])nodeNumbers;

            HashSet<Node> nodes = new HashSet<Node>();

            foreach (int n in nodeNumbersList) { _ = nodes.Add(GetNode(geometry, n)); }

            return nodes;
        }

        public static IEnumerable<Node> GetPlateIncidence(this OSGeometryCore geometry, int plateNumber)
        {
            HashSet<Node> plateIncidence = new HashSet<Node>(4);

            object nodeAId = 0; object nodeBId = 0; object nodeCId = 0; object nodeDId = 0;

            geometry.ComObject.GetPlateIncidence(plateNumber, ref nodeAId, ref nodeBId, ref nodeCId, ref nodeDId);

            _ = plateIncidence.Add(GetNode(geometry, (int)nodeAId));
            _ = plateIncidence.Add(GetNode(geometry, (int)nodeBId));
            _ = plateIncidence.Add(GetNode(geometry, (int)nodeCId));
            _ = plateIncidence.Add(GetNode(geometry, (int)nodeDId));

            return plateIncidence.Where(n => n != null);
        }

        public static Plate GetPlate(this OSGeometryCore geometry, int plateId)
        {
            Plate plate = new Plate();

            object nodeAId = 0; object nodeBId = 0; object nodeCId = 0; object nodeDId = 0;

            geometry.ComObject.GetPlateIncidence(plateId, ref nodeAId, ref nodeBId, ref nodeCId, ref nodeDId);

            plate.Id = plateId;
            plate.A = GetNode(geometry, (int)nodeAId);
            plate.B = GetNode(geometry, (int)nodeBId);
            plate.C = GetNode(geometry, (int)nodeCId);
            plate.D = GetNode(geometry, (int)nodeDId);

            return plate;
        }

        public static Beam GetBeam(this OSGeometryCore geometry, int beamNumber)
        {
            Beam beam = null;

            if (beamNumber != 0)
            {
                object startNodeId = 0;
                object endNodeId = 0;

                geometry.ComObject.GetMemberIncidence(beamNumber, ref startNodeId, ref endNodeId);
                Node startNode = GetNode(geometry, (int)startNodeId);
                Node endNode = GetNode(geometry, (int)endNodeId);

                beam = new Beam(beamNumber, startNode, endNode);
            }

            return beam;
        }

        public static Node GetNode(this OSGeometryCore geometry, int nodeNumber)
        {
            Node node = null;

            if (nodeNumber != 0)
            {
                object x = 0.0; object y = 0.0; object z = 0.0;

                geometry.ComObject.GetNodeCoordinates(nodeNumber, ref x, ref y, ref z);

                node = new Node(nodeNumber, (double)x, (double)y, (double)z);
            }

            return node;
        }

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
