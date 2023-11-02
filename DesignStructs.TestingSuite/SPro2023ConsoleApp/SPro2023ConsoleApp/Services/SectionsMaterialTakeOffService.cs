using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Models;

using SPro2023ConsoleApp.Models;

namespace SPro2023ConsoleApp.Services
{
    public class SectionsMaterialTakeOffService
    {
        #region Public Functions

        public static Dictionary<int, BeamMTOTableRow> Generate(StaadModelWrapper wrapper)
        {
            OpenSTAAD instance = wrapper.StaadInstance;

            object staadFile = string.Empty;

            instance.GetSTAADFile(ref staadFile, "TRUE");

            if (!string.IsNullOrWhiteSpace((string)staadFile))
            {
                OSPropertyUI property = instance.Property;
                OSGeometryUI geometry = instance.Geometry;

                int[] beams = GetAllBeamsList(geometry);

                Dictionary<int, BeamMTOTableRow> propertiesTable = SegregateBeamsByPropertyIds(property, beams);
                propertiesTable = FillSectionNames(property, propertiesTable);
                propertiesTable.Values.ToList().ForEach(r => r.EntitiesIds.ForEach(e => r.TotalLength += Math.Round(geometry.GetBeamLength(e), 3)));
                propertiesTable = FillTotalWeights(property, propertiesTable);

                return propertiesTable;
            }

            return null;
        }

        #endregion

        #region Private Helpers

        private static int[] GetAllBeamsList(OSGeometryUI geometry)
        {
            int nBeams = geometry.GetMemberCount();
            object beams = new int[nBeams];
            geometry.GetBeamList(ref beams);

            return (int[])beams;
        }

        private static Dictionary<int, BeamMTOTableRow> SegregateBeamsByPropertyIds(OSPropertyUI property, int[] beams)
        {
            int nRows = property.GetSectionPropertyCount();

            Dictionary<int, BeamMTOTableRow> propertiesTable = new Dictionary<int, BeamMTOTableRow>(nRows);

            foreach (int beamId in beams)
            {
                int propertyId = property.GetBeamSectionPropertyRefNo(beamId);

                if (!propertiesTable.TryGetValue(propertyId, out BeamMTOTableRow row))
                {
                    row = new BeamMTOTableRow { EntitiesIds = new List<int>() };
                    propertiesTable.Add(propertyId, row);
                }

                row.EntitiesIds.Add(beamId);
            }

            propertiesTable = new Dictionary<int, BeamMTOTableRow>(propertiesTable.OrderBy(pair => pair.Key)
                                  .ToDictionary(pair => pair.Key, pair => pair.Value));

            return propertiesTable;
        }

        private static Dictionary<int, BeamMTOTableRow> FillSectionNames(OSPropertyUI property, Dictionary<int, BeamMTOTableRow> propertiesTable)
        {
            foreach (int propertyId in propertiesTable.Keys)
            {
                object propertyName = string.Empty;

                property.GetSectionPropertyName(propertyId, ref propertyName);
                propertiesTable[propertyId].PropertyName = (string)propertyName;
            }

            return propertiesTable;
        }

        private static Dictionary<int, BeamMTOTableRow> FillTotalWeights(OSPropertyUI property, Dictionary<int, BeamMTOTableRow> propertiesTable)
        {
            foreach (int propertyId in propertiesTable.Keys)
            {
                BeamMTOTableRow row = propertiesTable[propertyId];

                int count = property.GetCountofSectionPropertyValuesEx();
                object propertyValues = new double[count];
                object propertyType = 0;

                property.GetSectionPropertyValuesEx(propertyId, ref propertyType, ref propertyValues);

                /// TODO:Notes:
                ///     1. First value in the propertyValues gives sectional area for most of the section types.
                ///        Introduce additional conditions or use a separate class to retrieve the sectional area for sections
                ///        which does not contain the sectional area in the propertyValues array.
                ///     2. To calculate the total weight multiply the total length with sectional area and then with density of the material.
                ///        In the below line of code a value 7.85 (t/m3) is used directly for testing purposes.
                ///        Actual weight has to be calculated using the density that is specific to the material selected.

                row.TotalWeight = Math.Round(((double[])propertyValues)[0] * row.TotalLength * 7.85, 3);

            }

            return propertiesTable;
        }

        #endregion
    }
}
