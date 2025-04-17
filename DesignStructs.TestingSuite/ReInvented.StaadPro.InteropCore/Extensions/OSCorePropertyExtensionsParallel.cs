using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCorePropertyExtensionsParallel
    {
        #region Fluent Functions

        public static OSCoreProperty CreateAndAssignSectionProperties(this OSCoreProperty property, HashSet<Beam> beams, int nThreads)
        {
            property.ComObject.CreateAndAssignSectionProperties(beams, nThreads);
            return property;
        }

        public static OSCoreProperty CreateAndAssignOffsetSpecifications(this OSCoreProperty property, HashSet<Beam> beams, int nThreads)
        {
            property.ComObject.CreateAndAssignOffsetSpecifications(beams, nThreads);
            return property;
        }

        public static OSCoreProperty AssignSectionProperties(this OSCoreProperty property, HashSet<SectionProperty> sectionProperties, int nThreads)
        {
            property.ComObject.AssignSectionProperties(sectionProperties, nThreads);
            return property;
        }

        public static OSCoreProperty AssignSectionProperty(this OSCoreProperty property, SectionProperty sectionProperty, int nThreads)
        {
            property.ComObject.AssignSectionProperty(sectionProperty, nThreads);
            return property;
        }

        public static OSCoreProperty AssignThicknessProperties(this OSCoreProperty property, HashSet<PlateThickness> thicknessPropertyCollection, int nThreads)
        {
            property.ComObject.AssignThicknessProperties(thicknessPropertyCollection, nThreads);
            return property;
        }

        public static OSCoreProperty AssignThicknessProperty(this OSCoreProperty property, PlateThickness thicknessProperty, int nThreads)
        {
            property.ComObject.AssignThicknessProperty(thicknessProperty, nThreads);
            return property;
        }

        public static OSCoreProperty AssignOffsetSpecifications(this OSCoreProperty property, HashSet<OffsetSpecification> offsetSpecificationCollection, int nThreads)
        {
            property.ComObject.AssignOffsetSpecifications(offsetSpecificationCollection, nThreads);
            return property;
        }

        public static OSCoreProperty AssignOffsetSpecification(this OSCoreProperty property, OffsetSpecification os, int nThreads)
        {
            property.ComObject.AssignOffsetSpecification(os, nThreads);
            return property;
        }

        public static OSCoreProperty AssignMaterialProperties(this OSCoreProperty property, HashSet<MaterialProperty> materialPropertyCollection, int nThreads)
        {
            property.ComObject.AssignMaterialProperties(materialPropertyCollection, nThreads);
            return property;
        }

        public static OSCoreProperty AssignMaterialProperty(this OSCoreProperty property, MaterialProperty mp, int nThreads)
        {
            property.ComObject.AssignMaterialProperty(mp, nThreads);
            return property;
        }

        public static OSCoreProperty AssignBetaAnglesToBeams(this OSCoreProperty property, HashSet<Beam> beams, int nThreads)
        {
            property.ComObject.AssignBetaAnglesToBeams(beams, nThreads);
            return property;
        }

        public static OSCoreProperty CreateAndAssignMemberEndReleases(this OSCoreProperty property, HashSet<Beam> beams, int nThreads)
        {
            property.ComObject.CreateAndAssignMemberEndReleases(beams, nThreads);
            return property;
        } 

        #endregion
    }
}
