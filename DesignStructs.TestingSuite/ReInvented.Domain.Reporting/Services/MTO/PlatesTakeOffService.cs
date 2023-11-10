using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Domain.Reporting.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.Services
{
    public class PlatesTakeOffService
    {
        #region Public Functions

        public static Dictionary<int, PlateMtoRow> Generate(StaadModelWrapper wrapper)
        {
            string staadFile = wrapper.StaadInstance.GetStaadFileNameOnly();

            if (!string.IsNullOrWhiteSpace(staadFile))
            {
                OSPropertyUI property = wrapper.StaadInstance.Property;
                OSGeometryUI geometry = wrapper.StaadInstance.Geometry;

                HashSet<Plate> plates = geometry.GetAllPlates();

                Dictionary<int, PlateMtoRow> propertiesTable = SegregatePlatesByPropertyIds(property, plates);
                propertiesTable = FillPlateThicknesses(property, propertiesTable);
                propertiesTable = FillTotalPlanAreas(wrapper, propertiesTable);
                propertiesTable = FillMaterialGrades(property, propertiesTable);

                propertiesTable.Values.ToList().ForEach(row => row.TotalWeight = Math.Round(row.TotalPlanArea * row.Thickness * (row.MaterialGrade.Density / 1000), 3));

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

                _ = row.Plates.Add(plate);
            }

            propertiesTable = new Dictionary<int, PlateMtoRow>(propertiesTable.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value));

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

        private static Dictionary<int, PlateMtoRow> FillMaterialGrades(OSPropertyUI property, Dictionary<int, PlateMtoRow> propertiesTable)
        {
            foreach (PlateMtoRow row in propertiesTable.Values)
            {
                row.MaterialGrade = MaterialsRepository.Instance.GetMaterialGradeFrom(property.GetPlateMaterialName(row.Plates.FirstOrDefault().Id));
            }

            return propertiesTable;
        }

        #endregion
    }
}
