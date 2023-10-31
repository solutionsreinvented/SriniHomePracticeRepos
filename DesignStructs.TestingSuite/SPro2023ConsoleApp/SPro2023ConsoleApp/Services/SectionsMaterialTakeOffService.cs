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
    public class SectionsMaterialTakeOffService : MaterialTakeOffService, IMaterialTakeOffService
    {
        public override void GenerateMTO(StaadModelWrapper wrapper)
        {
            OpenSTAAD instance = wrapper.StaadInstance;
            
            object staadFile = string.Empty;

            instance.GetSTAADFile(ref staadFile, "TRUE");

            if (!string.IsNullOrWhiteSpace((string)staadFile))
            {
                //Get member data
                OSPropertyUI property = instance.Property;
                OSGeometryUI geometry = instance.Geometry;

                int nBeams = geometry.GetMemberCount();
                object beams = new int[nBeams];
                geometry.GetBeamList(ref beams);

                int nRows = property.GetSectionPropertyCount();

                Dictionary<int, BeamMTOTableRow> propertiesTable = new Dictionary<int, BeamMTOTableRow>(nRows);

                int propertyId;

                int[] beamsArray = (int[])beams;

                for (int iBeam = 0; iBeam < nBeams; iBeam++)
                {
                    int beamId = beamsArray[iBeam];

                    propertyId = property.GetBeamSectionPropertyRefNo(beamId);

                    BeamMTOTableRow row;

                    if (propertiesTable.Keys.Contains(propertyId))
                    {
                        row = propertiesTable[propertyId];
                    }
                    else
                    {
                        row = new BeamMTOTableRow();
                        propertiesTable.Add(propertyId, row);
                    }

                    row.ObjectsCount += 1;
                    row.TotalLength += Math.Round(geometry.GetBeamLength(beamId), 3);
                }

                for (int iRow = 0; iRow < nRows; iRow++)
                {
                    propertyId = propertiesTable.Keys.ToList()[iRow];
                    BeamMTOTableRow row = propertiesTable[propertyId];

                    object propertyName = string.Empty;

                    property.GetSectionPropertyName(propertyId, ref propertyName);

                    row.PropertyName = (string)propertyName;


                    int count = property.GetCountofSectionPropertyValuesEx();
                    object propertyValues = new double[count];
                    object propertyType = 0;

                    property.GetSectionPropertyValuesEx(propertyId, ref propertyType, ref propertyValues);

                    /// Notes:
                    ///     1. First value in the propertyValues gives sectional area for most of the section types.
                    ///        Introduce additional conditions or use a separate class to retrieve the sectional area for sections
                    ///        which does not contain the sectional area in the propertyValues array.
                    ///     2. To calculate the multiply the total length with sectional area and then with density of the material.
                    ///        In the below line of code a value 7.85 (t/m3) is used directly for testing purposes.
                    ///        Actual weight has to be calculated using the density that is specific to the material selected.

                    row.TotalWeight = Math.Round(((double[])propertyValues)[0] * row.TotalLength * 7.85, 3);
                }

            }
        }
    }
}
