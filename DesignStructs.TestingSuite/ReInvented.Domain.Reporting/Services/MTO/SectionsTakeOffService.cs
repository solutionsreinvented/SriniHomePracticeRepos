using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Domain.Reporting.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.StaadPro.Interactivity.Services;

namespace ReInvented.Domain.Reporting.Services
{

    public class SectionsTakeOffService
    {
        #region Public Functions

        public static Dictionary<int, SectionMtoRow> Generate(StaadModelWrapper wrapper)
        {
            OpenSTAAD instance = wrapper.StaadInstance;

            object staadFile = string.Empty;

            instance.GetSTAADFile(ref staadFile, "TRUE");

            if (!string.IsNullOrWhiteSpace((string)staadFile))
            {
                OSPropertyUI property = instance.Property;
                OSGeometryUI geometry = instance.Geometry;

                HashSet<Beam> beams = StaadGeometryServices.GetAllBeams(geometry);

                Dictionary<int, SectionMtoRow> propertiesTable = SegregateBeamsByPropertyIds(property, beams);
                propertiesTable = FillSectionNames(property, propertiesTable);
                propertiesTable = FillMaterialGrades(property, propertiesTable);
                propertiesTable = FillTotalWeights(property, propertiesTable);

                return propertiesTable;
            }

            return null;
        }

        #endregion

        #region Private Helpers

        private static Dictionary<int, SectionMtoRow> SegregateBeamsByPropertyIds(OSPropertyUI property, HashSet<Beam> beams)
        {
            int nRows = property.GetSectionPropertyCount();

            Dictionary<int, SectionMtoRow> propertiesTable = new Dictionary<int, SectionMtoRow>(nRows);

            foreach (Beam beam in beams)
            {
                int propertyId = property.GetBeamSectionPropertyRefNo(beam.Id);

                if (!propertiesTable.TryGetValue(propertyId, out SectionMtoRow row))
                {
                    row = new SectionMtoRow();
                    propertiesTable.Add(propertyId, row);
                }

                _ = row.Beams.Add(beam);
            }

            propertiesTable = new Dictionary<int, SectionMtoRow>(propertiesTable.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value));

            return propertiesTable;
        }

        private static Dictionary<int, SectionMtoRow> FillSectionNames(OSPropertyUI property, Dictionary<int, SectionMtoRow> propertiesTable)
        {
            foreach (int propertyId in propertiesTable.Keys)
            {
                object propertyName = string.Empty;

                property.GetSectionPropertyName(propertyId, ref propertyName);
                propertiesTable[propertyId].PropertyName = (string)propertyName;
            }

            return propertiesTable;
        }

        private static Dictionary<int, SectionMtoRow> FillMaterialGrades(OSPropertyUI property, Dictionary<int, SectionMtoRow> propertiesTable)
        {
            foreach (SectionMtoRow row in propertiesTable.Values)
            {
                row.MaterialGrade = MaterialsRepository.Instance.GetMaterialGradeFrom(property.GetBeamMaterialName(row.Beams.FirstOrDefault().Id));
            }

            return propertiesTable;
        }

        private static Dictionary<int, SectionMtoRow> FillTotalWeights(OSPropertyUI property, Dictionary<int, SectionMtoRow> propertiesTable)
        {
            foreach (int propertyId in propertiesTable.Keys)
            {
                SectionMtoRow row = propertiesTable[propertyId];

                int count = property.GetCountofSectionPropertyValuesEx();
                object propertyValues = new double[count];
                object propertyType = 0;

                property.GetSectionPropertyValuesEx(propertyId, ref propertyType, ref propertyValues);
                row.SectionalArea = ((double[])propertyValues)[0];

                /// TODO: Notes:
                ///     1. First value in the propertyValues gives sectional area for most of the section types.
                ///        Introduce additional conditions or use a separate class to retrieve the sectional area for sections
                ///        which does not contain the sectional area in the propertyValues array.

                row.TotalWeight = Math.Round(row.SectionalArea * row.TotalLength * (row.MaterialGrade.Density / 1000), 3);

            }

            return propertiesTable;
        }

        #endregion
    }
}
