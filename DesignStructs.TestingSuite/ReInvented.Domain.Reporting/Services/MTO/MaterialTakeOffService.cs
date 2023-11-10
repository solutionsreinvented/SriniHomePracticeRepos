﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using OpenSTAADUI;

using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Tass.Enums;
using ReInvented.Shared;
using ReInvented.Shared.Attributes;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.Services
{
    public class MaterialTakeOffService
    {
        #region Public Functions

        public static MaterialTakeOff Generate(StaadModelWrapper wrapper)
        {
            OSGeometryUI geometry = wrapper.StaadInstance.Geometry;

            MaterialTakeOff mto = new MaterialTakeOff();

            if (wrapper == null)
            {
                throw new ArgumentNullException($"{nameof(wrapper)} shall not be null.");
            }

            OSPropertyUI property = wrapper.StaadInstance.Property;

            mto.SectionsRows = SectionsTakeOffService.Generate(wrapper);
            mto.PlatesRows = PlatesTakeOffService.Generate(wrapper);

            mto.BeamEntityGroups = geometry.GetEntityGroupsOfType<Beam>();
            mto.PlateEntityGroups = geometry.GetEntityGroupsOfType<Plate>();

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
            PropertyWiseSummary propertyWiseSummary = new PropertyWiseSummary() { };

            List<SectionsSummaryItem> sectionsSummaryItems = new List<SectionsSummaryItem>();
            List<PlatesSummaryItem> platesSummaryItems = new List<PlatesSummaryItem>();


            foreach (SectionMtoRow item in mto.SectionsRows.Values)
            {
                sectionsSummaryItems.AddRange(SegregateSectionsByDescription(item, mto.BeamEntityGroups.Where(g => g.MTODescription != "Unidentified"), sectionsSummaryItems.Count));
            }

            foreach (PlateMtoRow item in mto.PlatesRows.Values)
            {
                platesSummaryItems.AddRange(SegregatePlatesByDescription(item, mto.PlateEntityGroups.Where(g => g.MTODescription != "Unidentified"), platesSummaryItems.Count));
            }

            propertyWiseSummary.SectionsItems = sectionsSummaryItems.ToHashSet();
            propertyWiseSummary.PlatesItems = platesSummaryItems.ToHashSet();

            return propertyWiseSummary;
        }

        private static OverallSummary GenerateOverallSummary(PropertyWiseSummary propertyWiseSummary)
        {
            List<AssemblyGroupAttribute> assemblyGroups = EnumExtensions.GetAssemblyGroups<FeatureType>().ToList();

            OverallSummary overallSummary = new OverallSummary() { Items = new HashSet<OverallSummaryItem>() };

            int slNumber = 0;

            foreach (AssemblyGroupAttribute item in assemblyGroups.Skip(1).OrderBy(g => g.Order))
            {
                if (propertyWiseSummary.SectionsItems?.Count(i => i.AssemblyGroup == item.Group) > 0 || propertyWiseSummary.PlatesItems?.Count(i => i.AssemblyGroup == item.Group) > 0)
                {
                    slNumber++;

                    OverallSummaryItem summaryItem = new OverallSummaryItem()
                    {
                        SlNo = slNumber,
                        AssemblyGroup = item.Group,
                        Description = item.Group,
                    };

                    if (propertyWiseSummary.SectionsItems != null)
                    {
                        summaryItem.Weight += propertyWiseSummary.SectionsItems.Where(i => i.AssemblyGroup == item.Group).Sum(i => i.Weight);
                    }
                    if (propertyWiseSummary.PlatesItems != null)
                    {
                        summaryItem.Weight += propertyWiseSummary.PlatesItems.Where(i => i.AssemblyGroup == item.Group).Sum(i => i.Weight);
                    }

                    _ = overallSummary.Items.Add(summaryItem);
                }
            }

            return overallSummary;
        }

        #endregion

        #region Private Helpers

        private static string GetPcdFromGroupName(string groupName)
        {
            string pattern = @"PCD(\d+)";
            Match match = Regex.Match(groupName, pattern);

            return match.Value;
        }
        private static HashSet<SectionsSummaryItem> SegregateSectionsByDescription(SectionMtoRow row, IEnumerable<EntityGroup<Beam>> beamEntityGroups, int currentSlNo)
        {
            Dictionary<string, SectionsSummaryItem> descriptionItemPairs = new Dictionary<string, SectionsSummaryItem>();

            foreach (Beam b in row.Beams)
            {
                EntityGroup<Beam> matchedGroup = beamEntityGroups.FirstOrDefault(g => g.Entities.Contains(b));

                string mtoDescription = string.Empty;

                if (matchedGroup.GroupName.ToUpper().Contains("PCD"))
                {
                    string pcd = GetPcdFromGroupName(matchedGroup.GroupName);
                    mtoDescription = $"{matchedGroup.MTODescription} ({pcd})";
                }
                else
                {
                    mtoDescription = matchedGroup.MTODescription;
                }

                string assemblyGroup = EnumExtensions.ParseEnumFromGroupName<FeatureType>(matchedGroup.GroupName).GetAssemblyGroup();

                if (!descriptionItemPairs.TryGetValue(mtoDescription, out SectionsSummaryItem item))
                {
                    item = CreateNewSectionsSummaryItem(row.MaterialGrade.Designation, currentSlNo + 1, mtoDescription, assemblyGroup, row.PropertyName);
                    descriptionItemPairs.Add(mtoDescription, item);
                }

                item.Length += Math.Round(b.Length, 3);
                item.Weight += Math.Round(row.SectionalArea * b.Length * row.MaterialGrade.Density / 1000, 3);
            }

            return descriptionItemPairs.Values.ToHashSet();
        }

        private static HashSet<PlatesSummaryItem> SegregatePlatesByDescription(PlateMtoRow row, IEnumerable<EntityGroup<Plate>> plateEntityGroups, int currentSlNo)
        {
            Dictionary<string, PlatesSummaryItem> descriptionItemPairs = new Dictionary<string, PlatesSummaryItem>();

            foreach (Plate p in row.Plates)
            {
                EntityGroup<Plate> matchedGroup = plateEntityGroups.FirstOrDefault(g => g.Entities.Contains(p));

                string mtoDescription = matchedGroup.MTODescription;
                string assemblyGroup = EnumExtensions.ParseEnumFromGroupName<FeatureType>(matchedGroup.GroupName).GetAssemblyGroup();

                if (!descriptionItemPairs.TryGetValue(mtoDescription, out PlatesSummaryItem item))
                {
                    item = CreateNewPlatesSummaryItem(row.MaterialGrade.Designation, currentSlNo + 1, mtoDescription, assemblyGroup, $"Plate {row.Thickness * 1000} THK.");
                    descriptionItemPairs.Add(mtoDescription, item);
                }

                item.Area += Math.Round(p.Area, 3);
                item.Weight += Math.Round(row.Thickness * p.Area * row.MaterialGrade.Density / 1000, 3);
            }

            return descriptionItemPairs.Values.ToHashSet();
        }

        private static SectionsSummaryItem CreateNewSectionsSummaryItem(string materialGrade, int slNo, string description, string assemblyGroup, string section)
        {
            return new SectionsSummaryItem()
            {
                SlNo = slNo,
                Description = description,
                AssemblyGroup = assemblyGroup,
                MaterialGrade = materialGrade,
                Section = section
            };
        }

        private static PlatesSummaryItem CreateNewPlatesSummaryItem(string materialGrade, int slNo, string description, string assemblyGroup, string thickness)
        {
            return new PlatesSummaryItem()
            {
                SlNo = slNo,
                Description = description,
                AssemblyGroup = assemblyGroup,
                MaterialGrade = materialGrade,
                Thickness = thickness
            };
        }

        #endregion
    }
}
