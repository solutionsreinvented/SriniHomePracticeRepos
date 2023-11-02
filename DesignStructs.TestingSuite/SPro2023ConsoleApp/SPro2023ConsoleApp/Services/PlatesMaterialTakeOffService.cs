using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Models;

using SPro2023ConsoleApp.Models;

namespace SPro2023ConsoleApp.Services
{
    public class PlatesMaterialTakeOffService
    {
        #region Public Functions

        public static Dictionary<int, PlateMTOTableRow> Generate(StaadModelWrapper wrapper)
        {
            object staadFile = string.Empty;

            wrapper.StaadInstance.GetSTAADFile(ref staadFile, "TRUE");

            if (!string.IsNullOrWhiteSpace((string)staadFile))
            {
                OSPropertyUI property = wrapper.StaadInstance.Property;
                OSGeometryUI geometry = wrapper.StaadInstance.Geometry;

                int[] plates = GetAllPlatesList(geometry);

                Dictionary<int, PlateMTOTableRow> propertiesTable = SegregatePlatesByPropertyIds(property, plates);
                propertiesTable = FillPlateThicknesses(property, propertiesTable);
                propertiesTable = FillTotalPlanAreas(wrapper, propertiesTable);

                /// TODO: Notes:
                ///     1. First value in the propertyValues gives sectional area for most of the section types.
                ///        Introduce additional conditions or use a separate class to retrieve the sectional area for sections
                ///        which does not contain the sectional area in the propertyValues array.
                ///     2. To calculate the total weight multiply the total plan area with thickness and then with density of the material.
                ///        In the below line of code a value 7.85 (t/m³) is used directly for testing purposes.
                ///        Actual weight has to be calculated using the density that is specific to the material selected.

                propertiesTable.Values.ToList().ForEach(p => p.TotalWeight = Math.Round(p.TotalPlanArea * p.Thickness * 7.85, 3));

                return propertiesTable;
            }

            return null;
        }

        #endregion

        #region Private Functions

        private static int[] GetAllPlatesList(OSGeometryUI geometry)
        {
            int nPlates = geometry.GetPlateCount();
            object plates = new int[nPlates];
            geometry.GetPlateList(ref plates);

            return (int[])plates;
        }

        private static Dictionary<int, PlateMTOTableRow> SegregatePlatesByPropertyIds(OSPropertyUI property, int[] plates)
        {
            int nRows = property.GetThicknessPropertyCount();

            Dictionary<int, PlateMTOTableRow> propertiesTable = new Dictionary<int, PlateMTOTableRow>(nRows);

            foreach (int plateId in plates)
            {
                int propertyId = property.GetPlateThicknessPropertyRefNo(plateId);

                if (!propertiesTable.TryGetValue(propertyId, out PlateMTOTableRow row))
                {
                    row = new PlateMTOTableRow { EntitiesIds = new List<int>() };
                    propertiesTable.Add(propertyId, row);
                }

                row.EntitiesIds.Add(plateId);
            }

            propertiesTable = new Dictionary<int, PlateMTOTableRow>(propertiesTable.OrderBy(pair => pair.Key)
                                  .ToDictionary(pair => pair.Key, pair => pair.Value));

            return propertiesTable;
        }

        private static Dictionary<int, PlateMTOTableRow> FillPlateThicknesses(OSPropertyUI property, Dictionary<int, PlateMTOTableRow> propertiesTable)
        {
            foreach (PlateMTOTableRow row in propertiesTable.Values)
            {
                object plateThickness = new double[4];
                property.GetPlateThickness(row.EntitiesIds.FirstOrDefault(), ref plateThickness);
                row.Thickness = Math.Round(((double[])plateThickness).Average(), 5);
            }

            return propertiesTable;
        }

        private static Dictionary<int, PlateMTOTableRow> FillTotalPlanAreas(StaadModelWrapper wrapper, Dictionary<int, PlateMTOTableRow> propertiesTable)
        {
            OSPropertyUI property = wrapper.StaadInstance.Property;

            foreach (int propertyId in propertiesTable.Keys)
            {
                PlateMTOTableRow row = propertiesTable[propertyId];

                int propertyAssignedCount = property.GetThicknessPropertyAssignedPlateCount(propertyId);

                object propertyAssignedList = new int[propertyAssignedCount];

                property.GetThicknessPropertyAssignedPlateList(propertyId, ref propertyAssignedList);

                row.TotalPlanArea = PlateSurfaceAreaService.Calculate(wrapper, ((int[])propertyAssignedList).ToHashSet());
            }

            return propertiesTable;
        }

        #endregion
    }
}
