using System;
using System.Collections.Generic;
using System.Linq;

using DevDrive.Models;

using OpenSTAADUI;

using ReInvented.Sections.Domain.Repositories;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;

namespace DevDrive.Extensions
{
    public static class OSPropertyExtensions
    {
        public static int[] GetSectionPropertyList(this OSPropertyUI property)
        {
            dynamic spCount = property.GetSectionPropertyCount();
            object objSpList = new int[spCount];
            property.GetSectionPropertyList(ref objSpList);

            return (int[])objSpList;
        }

        public static int[] GetThicknessPropertyList(this OSPropertyUI property)
        {
            dynamic tpCount = property.GetThicknessPropertyCount();
            object objTpList = new int[tpCount];
            property.GetThicknessPropertyList(ref objTpList);

            return (int[])objTpList;
        }

        public static HashSet<Beam> GetSectionPropertyAssignedBeams(this OSPropertyUI property, HashSet<Beam> allBeams, int propertyId)
        {
            dynamic bCount = property.GetSectionPropertyAssignedBeamCount(propertyId);
            object objBList = new int[bCount];
            property.GetSectionPropertyAssignedBeamList(propertyId, ref objBList);

            int[] beamList = (int[])objBList;

            return allBeams.Where(b => beamList.Contains(b.Id)).ToHashSet();
        }

        public static HashSet<Plate> GetThicknessPropertyAssignedPlates(this OSPropertyUI property, HashSet<Plate> allPlates, int propertyId)
        {
            dynamic pCount = property.GetThicknessPropertyAssignedPlateCount(propertyId);
            object objPList = new int[pCount];
            property.GetThicknessPropertyAssignedPlateList(propertyId, ref objPList);

            int[] plateList = (int[])objPList;

            return allPlates.Where(p => plateList.Contains(p.Id)).ToHashSet();
        }

        public static HashSet<Beam> GetBeamsWithNoSectionProperty(this OSPropertyUI property, HashSet<Beam> allBeams, IEnumerable<int> propertyIds)
        {
            IEnumerable<Beam> unassignedBeams = allBeams;

            foreach (int pId in propertyIds)
            {
                HashSet<Beam> beams = property.GetSectionPropertyAssignedBeams(allBeams, pId);
                unassignedBeams = unassignedBeams.Except(beams);
            }

            return unassignedBeams.ToHashSet();
        }

        public static HashSet<Plate> GetPlatesWithNoThicknessProperty(this OSPropertyUI property, HashSet<Plate> allPlates, IEnumerable<int> propertyIds)
        {
            IEnumerable<Plate> unassignedPlates = allPlates;

            foreach (int pId in propertyIds)
            {
                HashSet<Plate> plates = property.GetThicknessPropertyAssignedPlates(allPlates, pId);
                unassignedPlates = unassignedPlates.Except(plates);
            }

            return unassignedPlates.ToHashSet();
        }

        public static string GetSectionPropertyName(this OSPropertyUI property, int propertyId)
        {
            object propertyName = string.Empty;

            property.GetSectionPropertyName(propertyId, ref propertyName);
            return (string)propertyName;
        }

        public static double[] GetPlateThickness(this OSPropertyUI property, int propertyId)
        {
            object plateThickness = new double[4];
            property.GetPlateThickness(propertyId, ref plateThickness);

            return (double[])plateThickness;
        }

        public static double GetAveragePlateThickness(this OSPropertyUI property, int propertyId)
        {
            return Math.Round(GetPlateThickness(property, propertyId).Average(), 5);
        }

        public static double[] GetSectionPropertyValues(this OSPropertyUI property, int propertyId)
        {
            int count = property.GetCountofSectionPropertyValuesEx();
            object propertyValues = new double[count];
            object propertyType = 0;

            property.GetSectionPropertyValuesEx(propertyId, ref propertyType, ref propertyValues);
            return (double[])propertyValues;
        }

        public static double GetSectionalArea(this OSPropertyUI property, int propertyId)
        {
            return property.GetSectionPropertyValues(propertyId)[0];
        }

        public static PlateMtoRow GetPlateMtoRow(this OSPropertyUI property, HashSet<Plate> allPlates, int propertyId)
        {
            List<Plate> plates = property.GetThicknessPropertyAssignedPlates(allPlates, propertyId).ToList();

            if (plates.Count() >= 1)
            {
                PlateMtoRow mtoRow = new PlateMtoRow(propertyId)
                {
                    Thickness = GetAveragePlateThickness(property, propertyId),
                    MaterialGrade = MaterialsRepository.Instance.GetMaterialGradeFrom(property.GetPlateMaterialName(plates.FirstOrDefault().Id))
                };

                plates.ForEach(p => mtoRow.Plates.Add(p));
                mtoRow.TotalWeight = Math.Round(mtoRow.TotalPlanArea * mtoRow.Thickness * (mtoRow.MaterialGrade.Density / 1000), 3);

                return mtoRow;
            }

            return null;
        }

        public static SectionMtoRow GetSectionMtoRow(this OSPropertyUI property, HashSet<Beam> allBeams, int propertyId)
        {
            HashSet<Beam> beams = property.GetSectionPropertyAssignedBeams(allBeams, propertyId);

            if (beams.Count() >= 1)
            {
                SectionMtoRow mtoRow = new SectionMtoRow(propertyId)
                {
                    MaterialGrade = MaterialsRepository.Instance.GetMaterialGradeFrom(property.GetBeamMaterialName(beams.FirstOrDefault().Id)),
                    PropertyName = property.GetSectionPropertyName(propertyId),
                    ///TODO: Retrieval of SectionalArea may depend on the property type. Implementation may need to be changed.
                    SectionalArea = property.GetSectionalArea(propertyId)
                };

                beams.ToList().ForEach(b => mtoRow.Beams.Add(b));
                mtoRow.TotalWeight = Math.Round(mtoRow.SectionalArea * mtoRow.TotalLength * (mtoRow.MaterialGrade.Density / 1000), 3);

                return mtoRow;
            }

            return null;
        }

        public static HashSet<SectionMtoRow> GetAllSectionMtoRows(this OSPropertyUI property, OSGeometryUI geometry)
        {
            HashSet<Beam> allBeams = geometry.GetAllEntities<Beam>(1);
            return property.GetAllSectionMtoRows(allBeams);
        }

        public static HashSet<SectionMtoRow> GetAllSectionMtoRows(this OSPropertyUI property, HashSet<Beam> allBeams)
        {
            int[] pPropIds = property.GetSectionPropertyList();

            if (pPropIds.Count() >= 1)
            {
                HashSet<SectionMtoRow> sectionMtoRows = new HashSet<SectionMtoRow>();

                foreach (int pPropId in pPropIds)
                {
                    _ = sectionMtoRows.Add(property.GetSectionMtoRow(allBeams, pPropId));
                }

                return sectionMtoRows.ToHashSet();
            }

            return null;
        }

        public static HashSet<PlateMtoRow> GetAllPlateMtoRows(this OSPropertyUI property, OSGeometryUI geometry)
        {
            HashSet<Plate> allPlates = geometry.GetAllEntities<Plate>(1);
            return property.GetAllPlateMtoRows(allPlates);
        }

        public static HashSet<PlateMtoRow> GetAllPlateMtoRows(this OSPropertyUI property, HashSet<Plate> allPlates)
        {
            int[] pPropIds = property.GetThicknessPropertyList();

            if (pPropIds.Count() >= 1)
            {
                HashSet<PlateMtoRow> plateMtoRows = new HashSet<PlateMtoRow>();

                foreach (int pPropId in pPropIds)
                {
                    _ = plateMtoRows.Add(property.GetPlateMtoRow(allPlates, pPropId));
                }

                return plateMtoRows.ToHashSet();
            }

            return null;
        }
    }
}
