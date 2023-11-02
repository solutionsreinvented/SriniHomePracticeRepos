using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.StaadPro.Interactivity.Services;

using SPro2023ConsoleApp.Models;

namespace SPro2023ConsoleApp.Services
{
    public class PlatesMaterialTakeOffService
    {
        #region Public Functions

        public static Dictionary<int, PlateMtoRow> Generate(StaadModelWrapper wrapper)
        {
            object staadFile = string.Empty;

            wrapper.StaadInstance.GetSTAADFile(ref staadFile, "TRUE");

            if (!string.IsNullOrWhiteSpace((string)staadFile))
            {
                OSPropertyUI property = wrapper.StaadInstance.Property;
                OSGeometryUI geometry = wrapper.StaadInstance.Geometry;

                HashSet<Plate> plates = StaadGeometryServices.GetAllPlates(geometry);

                Dictionary<int, PlateMtoRow> propertiesTable = SegregatePlatesByPropertyIds(property, plates);
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

        private static Dictionary<int, PlateMtoRow> SegregatePlatesByPropertyIds(OSPropertyUI property, HashSet<Plate> plates)
        {
            int nRows = property.GetThicknessPropertyCount();

            Dictionary<int, PlateMtoRow> propertiesTable = new Dictionary<int, PlateMtoRow>(nRows);

            foreach (Plate plate in plates)
            {
                int propertyId = property.GetPlateThicknessPropertyRefNo(plate.Id);

                if (!propertiesTable.TryGetValue(propertyId, out PlateMtoRow row))
                {
                    row = new PlateMtoRow();
                    propertiesTable.Add(propertyId, row);
                }

                row.Plates.Add(plate);
            }

            propertiesTable = new Dictionary<int, PlateMtoRow>(propertiesTable.OrderBy(pair => pair.Key)
                                  .ToDictionary(pair => pair.Key, pair => pair.Value));

            return propertiesTable;
        }

        private static Dictionary<int, PlateMtoRow> FillPlateThicknesses(OSPropertyUI property, Dictionary<int, PlateMtoRow> propertiesTable)
        {
            foreach (PlateMtoRow row in propertiesTable.Values)
            {
                object plateThickness = new double[4];
                property.GetPlateThickness(row.Plates.FirstOrDefault().Id, ref plateThickness);
                row.Thickness = Math.Round(((double[])plateThickness).Average(), 5);
            }

            return propertiesTable;
        }

        private static Dictionary<int, PlateMtoRow> FillTotalPlanAreas(StaadModelWrapper wrapper, Dictionary<int, PlateMtoRow> propertiesTable)
        {
            OSPropertyUI property = wrapper.StaadInstance.Property;

            foreach (int propertyId in propertiesTable.Keys)
            {
                int propertyAssignedCount = property.GetThicknessPropertyAssignedPlateCount(propertyId);

                object propertyAssignedList = new int[propertyAssignedCount];

                property.GetThicknessPropertyAssignedPlateList(propertyId, ref propertyAssignedList);
            }

            return propertiesTable;
        }

        #endregion
    }
}
