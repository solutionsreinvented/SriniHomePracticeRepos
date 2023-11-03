using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;
using ReInvented.Domain.Tass.Enums;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.StaadPro.Interactivity.Services;

namespace SPro2023ConsoleApp.Models
{
    public class MaterialTakeOffService
    {
        #region Public Functions

        public static MaterialTakeOff Generate(StaadModelWrapper wrapper)
        {
            MaterialTakeOff mto = new MaterialTakeOff();

            int beamGroupsCount = wrapper.Geometry.GetGroupCount(GroupType.Beams);

            object groups = new string[beamGroupsCount];

            wrapper.Geometry.GetGroupNames(GroupType.Beams, ref groups);


            if (wrapper == null)
            {
                throw new ArgumentNullException($"{nameof(wrapper)} shall not be null.");
            }

            OSPropertyUI property = wrapper.StaadInstance.Property;

            mto.SectionsRows = SectionsMaterialTakeOffService.Generate(wrapper);
            mto.PlatesRows = PlatesMaterialTakeOffService.Generate(wrapper);
            
            mto.BeamEntityGroups = StaadGeometryServices.GetEntityGroupsOfType<Beam>(wrapper.StaadInstance.Geometry);
            mto.PlateEntityGroups = StaadGeometryServices.GetEntityGroupsOfType<Plate>(wrapper.StaadInstance.Geometry);


            mto.BeamEntityGroups.ToList().ForEach(g => g.GroupDescription = EnumExtensions.ParseEnumFromDescription<FeatureType>(g.GroupName).GetMTODescription());
            mto.PlateEntityGroups.ToList().ForEach(g => g.GroupDescription = EnumExtensions.ParseEnumFromDescription<FeatureType>(g.GroupName).GetMTODescription());

            List<PropertyWiseSummaryItem> propertyWiseSummary = new List<PropertyWiseSummaryItem>();

            foreach (SectionMtoRow item in mto.SectionsRows.Values)
            {
                propertyWiseSummary.AddRange(SegregateSectionsByDescription(item, mto.BeamEntityGroups.Where(g => g.GroupDescription != "Unidentified"), propertyWiseSummary.Count));
            }

            foreach (PlateMtoRow item in mto.PlatesRows.Values)
            {
                propertyWiseSummary.AddRange(SegregatePlatesByDescription(item, mto.PlateEntityGroups.Where(g => g.GroupDescription != "Unidentified"), propertyWiseSummary.Count));
            }


            mto.PropertyWiseSummary = new PropertyWiseSummary() { Items = propertyWiseSummary.ToHashSet() };

            return mto;
        }

        #endregion

        private static HashSet<PropertyWiseSummaryItem> SegregateSectionsByDescription(SectionMtoRow row, IEnumerable<EntityGroup<Beam>> beamEntityGroups, int currentSlNo)
        {
            Dictionary<string, PropertyWiseSummaryItem> descriptionItemPairs = new Dictionary<string, PropertyWiseSummaryItem>();

            foreach (Beam b in row.Beams)
            {
                string description = beamEntityGroups.FirstOrDefault(g => g.Entities.Contains(b)).GroupDescription;

                if (!descriptionItemPairs.TryGetValue(description, out PropertyWiseSummaryItem item))
                {
                    item = new PropertyWiseSummaryItem()
                    {
                        SlNo = currentSlNo + 1,
                        Description = description,
                        MaterialGrade = row.MaterialGrade.Designation,
                        ProfileOrThickness = row.PropertyName
                    };

                    descriptionItemPairs.Add(description, item);
                }

                item.LengthOrArea += Math.Round(b.Length, 3);
                item.Weight += Math.Round(row.SectionalArea * b.Length * row.MaterialGrade.Density / 1000, 3);
            }

            return descriptionItemPairs.Values.ToHashSet();
        }

        private static HashSet<PropertyWiseSummaryItem> SegregatePlatesByDescription(PlateMtoRow row, IEnumerable<EntityGroup<Plate>> plateEntityGroups, int currentSlNo)
        {
            Dictionary<string, PropertyWiseSummaryItem> descriptionItemPairs = new Dictionary<string, PropertyWiseSummaryItem>();

            foreach (Plate p in row.Plates)
            {
                string description = plateEntityGroups.FirstOrDefault(g => g.Entities.Contains(p)).GroupDescription;

                if (!descriptionItemPairs.TryGetValue(description, out PropertyWiseSummaryItem item))
                {
                    item = new PropertyWiseSummaryItem()
                    {
                        SlNo = currentSlNo + 1,
                        Description = description,
                        MaterialGrade = row.MaterialGrade.Designation,
                        ProfileOrThickness = $"{row.Thickness * 1000} THK."
                    };

                    descriptionItemPairs.Add(description, item);
                }

                item.LengthOrArea += Math.Round(p.Area, 3);
                item.Weight += Math.Round(row.Thickness * p.Area * row.MaterialGrade.Density / 1000, 3);
            }

            return descriptionItemPairs.Values.ToHashSet();
        }
    }
}
