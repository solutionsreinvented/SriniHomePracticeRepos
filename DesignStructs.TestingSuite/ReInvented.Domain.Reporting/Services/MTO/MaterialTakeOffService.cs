using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Tass.Enums;
using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.StaadPro.Interactivity.Services.Staad;

namespace ReInvented.Domain.Reporting.Services
{
    public class MaterialTakeOffService
    {
        #region Public Functions

        public static MaterialTakeOff Generate(StaadModelWrapper wrapper)
        {
            MaterialTakeOff mto = new MaterialTakeOff();

            if (wrapper == null)
            {
                throw new ArgumentNullException($"{nameof(wrapper)} shall not be null.");
            }

            OSPropertyUI property = wrapper.StaadInstance.Property;

            mto.SectionsRows = SectionsTakeOffService.Generate(wrapper);
            mto.PlatesRows = PlatesTakeOffService.Generate(wrapper);

            mto.BeamEntityGroups = StaadGeometryServices.GetEntityGroupsOfType<Beam>(wrapper.StaadInstance.Geometry);
            mto.PlateEntityGroups = StaadGeometryServices.GetEntityGroupsOfType<Plate>(wrapper.StaadInstance.Geometry);

            mto.BeamEntityGroups.ToList().ForEach(g => g.MTODescription = EnumExtensions.ParseEnumFromGroupName<FeatureType>(g.GroupName).GetMTODescription());
            mto.PlateEntityGroups.ToList().ForEach(g => g.MTODescription = EnumExtensions.ParseEnumFromGroupName<FeatureType>(g.GroupName).GetMTODescription());

            mto.PropertyWiseSummary = GeneratePropertyWiseSummary(mto);

            mto.OverallSummary = GenerateOverallSummary(mto.PropertyWiseSummary);

            return mto;
        }

        #endregion

        #region Private Functions - Main

        private static PropertyWiseSummary GeneratePropertyWiseSummary(MaterialTakeOff mto)
        {
            PropertyWiseSummary propertyWiseSummary = new PropertyWiseSummary();

            List<PropertyWiseSummaryItem> propertyWiseSummaryItems = new List<PropertyWiseSummaryItem>();

            foreach (SectionMtoRow item in mto.SectionsRows.Values)
            {
                propertyWiseSummaryItems.AddRange(SegregateSectionsByDescription(item, mto.BeamEntityGroups.Where(g => g.MTODescription != "Unidentified"), propertyWiseSummaryItems.Count));
            }

            foreach (PlateMtoRow item in mto.PlatesRows.Values)
            {
                propertyWiseSummaryItems.AddRange(SegregatePlatesByDescription(item, mto.PlateEntityGroups.Where(g => g.MTODescription != "Unidentified"), propertyWiseSummaryItems.Count));
            }

            propertyWiseSummary.Items = propertyWiseSummaryItems.ToHashSet();

            return propertyWiseSummary;
        }

        private static OverallSummary GenerateOverallSummary(PropertyWiseSummary propertyWiseSummary)
        {
            List<string> overallSummayItems = new List<string>() { "Launder", "Wall", "Floor Plate", "Compression Ring", "Underflow Cone", "Center Column",
                                                                   "Support Structure", "Bridge", "Bolted Flanges" };

            OverallSummary overallSummary = new OverallSummary() { Items = new HashSet<OverallSummaryItem>() };

            int slNumber = 0;

            foreach (string item in overallSummayItems)
            {
                if (propertyWiseSummary.Items.Count(i => i.AssemblyGroup == item) > 0)
                {
                    slNumber++;

                    OverallSummaryItem summaryItem = new OverallSummaryItem()
                    {
                        SlNo = slNumber,
                        AssemblyGroup = item,
                        Description = item,
                        Weight = propertyWiseSummary.Items.Where(i => i.AssemblyGroup == item).Sum(i => i.Weight)
                    };

                    _ = overallSummary.Items.Add(summaryItem);
                }
            }

            return overallSummary;
        }

        #endregion

        #region Private Helpers

        private static HashSet<PropertyWiseSummaryItem> SegregateSectionsByDescription(SectionMtoRow row, IEnumerable<EntityGroup<Beam>> beamEntityGroups, int currentSlNo)
        {
            Dictionary<string, PropertyWiseSummaryItem> descriptionItemPairs = new Dictionary<string, PropertyWiseSummaryItem>();

            foreach (Beam b in row.Beams)
            {
                EntityGroup<Beam> matchedGroup = beamEntityGroups.FirstOrDefault(g => g.Entities.Contains(b));

                string mtoDescription = matchedGroup.MTODescription;
                string assemblyGroup = EnumExtensions.ParseEnumFromGroupName<FeatureType>(matchedGroup.GroupName).GetAssemblyGroup();

                if (!descriptionItemPairs.TryGetValue(mtoDescription, out PropertyWiseSummaryItem item))
                {
                    item = CreateNewPropertyWiseSummaryItem(row.MaterialGrade.Designation, currentSlNo + 1, mtoDescription, assemblyGroup, row.PropertyName);
                    descriptionItemPairs.Add(mtoDescription, item);
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
                EntityGroup<Plate> matchedGroup = plateEntityGroups.FirstOrDefault(g => g.Entities.Contains(p));

                string mtoDescription = matchedGroup.MTODescription;
                string assemblyGroup = EnumExtensions.ParseEnumFromGroupName<FeatureType>(matchedGroup.GroupName).GetAssemblyGroup();

                if (!descriptionItemPairs.TryGetValue(mtoDescription, out PropertyWiseSummaryItem item))
                {
                    item = CreateNewPropertyWiseSummaryItem(row.MaterialGrade.Designation, currentSlNo + 1, mtoDescription, assemblyGroup, $"Plate {row.Thickness * 1000} THK.");
                    descriptionItemPairs.Add(mtoDescription, item);
                }

                item.LengthOrArea += Math.Round(p.Area, 3);
                item.Weight += Math.Round(row.Thickness * p.Area * row.MaterialGrade.Density / 1000, 3);
            }

            return descriptionItemPairs.Values.ToHashSet();
        }

        private static PropertyWiseSummaryItem CreateNewPropertyWiseSummaryItem(string materialGrade, int slNo, string description, string assemblyGroup, string profileOrThickness)
        {
            return new PropertyWiseSummaryItem()
            {
                SlNo = slNo,
                Description = description,
                AssemblyGroup = assemblyGroup,
                MaterialGrade = materialGrade,
                ProfileOrThickness = profileOrThickness
            };
        }

        #endregion
    }
}
