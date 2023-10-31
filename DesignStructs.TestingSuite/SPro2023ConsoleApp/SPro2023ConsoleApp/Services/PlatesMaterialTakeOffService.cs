using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Models;

using SPro2023ConsoleApp.Base;
using SPro2023ConsoleApp.Interfaces;
using SPro2023ConsoleApp.Models;

namespace SPro2023ConsoleApp.Services
{
    public class PlatesMaterialTakeOffService : MaterialTakeOffService, IMaterialTakeOffService
    {
        public override void GenerateMTO(StaadModelWrapper wrapper)
        {
            object staadFile = string.Empty;

            wrapper.StaadInstance.GetSTAADFile(ref staadFile, "TRUE");

            if (!string.IsNullOrWhiteSpace((string)staadFile))
            {
                //Get member data
                OSPropertyUI property = wrapper.StaadInstance.Property;
                OSGeometryUI geometry = wrapper.StaadInstance.Geometry;

                int nPlates = geometry.GetPlateCount();
                object plates = new int[nPlates];
                geometry.GetPlateList(ref plates);

                int nRows = property.GetThicknessPropertyCount();

                Dictionary<int, PlateMTOTableRow> propertiesTable = new Dictionary<int, PlateMTOTableRow>(nRows);
                int propertyId;

                int[] PlatesArray = (int[])plates;

                for (int iPlate = 0; iPlate < nPlates; iPlate++)
                {
                    int PlateId = PlatesArray[iPlate];

                    propertyId = property.GetPlateThicknessPropertyRefNo(PlateId);

                    PlateMTOTableRow row;

                    if (propertiesTable.Keys.Contains(propertyId))
                    {
                        row = propertiesTable[propertyId];
                    }
                    else
                    {
                        row = new PlateMTOTableRow();
                        propertiesTable.Add(propertyId, row);
                    }

                    row.ObjectsCount += 1;
                    //row.TotalLength += Math.Round(property.Get.GetBeamLength(PlateId), 3);
                }

                for (int iRow = 0; iRow < nRows; iRow++)
                {
                    propertyId = propertiesTable.Keys.ToList()[iRow];
                    PlateMTOTableRow row = propertiesTable[propertyId];

                    object plateThickness = new double[4];

                    property.GetPlateThickness(propertyId, ref plateThickness);

                    int propertyAssignedCount = property.GetThicknessPropertyAssignedPlateCount(propertyId);

                    object propertyAssignedList = new int[propertyAssignedCount];

                    property.GetThicknessPropertyAssignedPlateList(propertyId, ref propertyAssignedList);

                    row.Thickness = Math.Round(((double[])plateThickness).Average(), 5);
                    row.TotalPlanArea = PlateSurfaceAreaService.Calculate(wrapper, ((int[])propertyAssignedList).ToHashSet());


                    /// Notes:
                    ///     1. First value in the propertyValues gives sectional area for most of the section types.
                    ///        Introduce additional conditions or use a separate class to retrieve the sectional area for sections
                    ///        which does not contain the sectional area in the propertyValues array.
                    ///     2. To calculate the multiply the total length with sectional area and then with density of the material.
                    ///        In the below line of code a value 7.85 (t/m³) is used directly for testing purposes.
                    ///        Actual weight has to be calculated using the density that is specific to the material selected.

                    row.TotalWeight = Math.Round(row.TotalPlanArea * row.Thickness * 7.85, 3);
                }
            }
        }
    }
}
