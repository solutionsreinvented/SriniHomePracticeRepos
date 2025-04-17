using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCorePropertyExtensions
    {
        #region New

        public static IGenericSection GetSectionFrom(this OSCoreProperty property, int propertyId)
        {
            return property.ComObject.GetSectionFrom(propertyId);
        }

        #endregion

        #region Regular Functions

        public static PlateElementThickness GetPlateThickness(this OSCoreProperty property, int plateId)
        {
            return property.ComObject.GetPlateThickness(plateId);
        }

        public static int[] GetSectionPropertyList(this OSCoreProperty property)
        {
            return property.ComObject.GetSectionPropertyList();
        }

        public static int[] GetThicknessPropertyList(this OSCoreProperty property)
        {
            return property.ComObject.GetThicknessPropertyList();
        }

        public static HashSet<Beam> GetSectionPropertyAssignedBeams(this OSCoreProperty property, HashSet<Beam> allBeams, int propertyId)
        {
            return property.ComObject.GetSectionPropertyAssignedBeams(allBeams, propertyId);
        }

        public static HashSet<Plate> GetThicknessPropertyAssignedPlates(this OSCoreProperty property, HashSet<Plate> allPlates, int propertyId)
        {
            return property.ComObject.GetThicknessPropertyAssignedPlates(allPlates, propertyId);
        }

        public static HashSet<Beam> GetBeamsWithMissingSectionProperty(this OSCoreProperty property, HashSet<Beam> allBeams, IEnumerable<int> propertyIds)
        {
            return property.ComObject.GetBeamsWithMissingSectionProperty(allBeams, propertyIds);
        }

        public static HashSet<Plate> GetPlatesWithMissingThicknessProperty(this OSCoreProperty property, HashSet<Plate> allPlates, IEnumerable<int> propertyIds)
        {
            return property.ComObject.GetPlatesWithMissingThicknessProperty(allPlates, propertyIds);
        }

        public static string GetSectionPropertyName(this OSCoreProperty property, int propertyId)
        {
            return property.ComObject.GetSectionPropertyName(propertyId);
        }

        public static double GetAveragePlateThickness(this OSCoreProperty property, int plateId)
        {
            return property.ComObject.GetAveragePlateThickness(plateId);
        }

        public static double[] GetSectionPropertyValues(this OSCoreProperty property, int propertyId)
        {
            return property.ComObject.GetSectionPropertyValues(propertyId);
        }

        public static double GetSectionalArea(this OSCoreProperty property, int propertyId)
        {
            return property.GetSectionPropertyValues(propertyId)[0];
        }

        #endregion

        #region Fluent Functions

        public static OSCoreProperty CreateAndAssignSectionProperties(this OSCoreProperty property, HashSet<Beam> beams)
        {
            property.ComObject.CreateAndAssignSectionProperties(beams);
            return property;
        }

        public static OSCoreProperty CreateAndAssignOffsetSpecifications(this OSCoreProperty property, HashSet<Beam> beams)
        {
            property.ComObject.CreateAndAssignOffsetSpecifications(beams);
            return property;
        }

        public static OSCoreProperty CreateSectionProperties(this OSCoreProperty property, HashSet<SectionProperty> sectionProperties)
        {
            property.ComObject.CreateSectionProperties(sectionProperties);
            return property;
        }

        public static OSCoreProperty CreateSectionProperty(this OSCoreProperty property, SectionProperty sectionProperty)
        {
            property.ComObject.CreateSectionProperty(sectionProperty);
            return property;
        }

        public static OSCoreProperty CreatePlateThicknessProperties(this OSCoreProperty property, HashSet<PlateThickness> thicknessPropertyCollection)
        {
            property.ComObject.CreatePlateThicknessProperties(thicknessPropertyCollection);
            return property;
        }

        public static OSCoreProperty CreatePlateThicknessProperty(OSCoreProperty property, PlateThickness thicknessProperty)
        {
            property.ComObject.CreatePlateThicknessProperty(thicknessProperty);
            return property;
        }

        public static OSCoreProperty CreateOffsetSpecifications(this OSCoreProperty property, HashSet<OffsetSpecification> offsetSpecificationCollection)
        {
            property.ComObject.CreateOffsetSpecifications(offsetSpecificationCollection);
            return property;
        }

        public static OSCoreProperty CreateOffsetSpecification(this OSCoreProperty property, OffsetSpecification specification)
        {
            property.ComObject.CreateOffsetSpecification(specification);
            return property;
        }

        public static OSCoreProperty CreateMaterialProperties(this OSCoreProperty property, HashSet<MaterialProperty> materialProperties)
        {
            property.ComObject.CreateMaterialProperties(materialProperties);
            return property;
        }

        public static OSCoreProperty CreateMaterialProperty(this OSCoreProperty property, MaterialProperty mp)
        {
            property.ComObject.CreateMaterialProperty(mp);
            return property;
        }

        public static OSCoreProperty AssignSectionProperties(this OSCoreProperty property, HashSet<SectionProperty> sectionProperties)
        {
            property.ComObject.AssignSectionProperties(sectionProperties);
            return property;
        }

        public static OSCoreProperty AssignSectionProperty(this OSCoreProperty property, SectionProperty sectionProperty)
        {
            property.ComObject.AssignSectionProperty(sectionProperty);
            return property;
        }

        public static OSCoreProperty AssignThicknessProperties(this OSCoreProperty property, HashSet<PlateThickness> thicknessPropertyCollection)
        {
            property.ComObject.AssignThicknessProperties(thicknessPropertyCollection);
            return property;
        }

        public static OSCoreProperty AssignOffsetSpecifications(this OSCoreProperty property, HashSet<OffsetSpecification> offsetSpecificationCollection)
        {
            property.ComObject.AssignOffsetSpecifications(offsetSpecificationCollection);
            return property;
        }

        public static OSCoreProperty AssignMaterialProperties(this OSCoreProperty property, HashSet<MaterialProperty> materialPropertyCollection)
        {
            property.ComObject.AssignMaterialProperties(materialPropertyCollection);
            return property;
        }

        public static OSCoreProperty AssignBetaAnglesToBeams(this OSCoreProperty property, HashSet<Beam> beams)
        {
            property.ComObject.AssignBetaAnglesToBeams(beams);
            return property;
        }

        public static OSCoreProperty CreateAndAssignMemberEndReleases(this OSCoreProperty property, HashSet<Beam> beams)
        {
            property.ComObject.CreateAndAssignMemberEndReleases(beams);
            return property;
        }

        #endregion
    }
}
