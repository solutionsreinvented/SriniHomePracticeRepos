using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ReInvented.DataAccess.Services;
using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.ExcelInteropDesign.Services
{
    public class SectionDesignService
    {
        #region Public Functions

        public static Dictionary<string, double> Design(IEnumerable<Classification> classifications, ISectionDesignData designData)
        {
            string filePath;
            Dictionary<string, double> allUtilizationRatios = new Dictionary<string, double>();

            List<IRolledSection> allSections = classifications.SelectMany(c => c.Sections).ToList();
            Dictionary<Type, List<IRolledSection>> segregatedSections = SegregateSectionsByType(allSections);

            foreach (Type key in segregatedSections.Keys)
            {
                filePath = Path.Combine(FileServiceProvider.ExcelTemplatesTestingDirectory, "Columns", ExcelTemplateNameProvider.GetName(key));
                List<IRolledSection> sections = segregatedSections[key];

                Dictionary<string, double> currentUtilizationRatios;

                if (key == typeof(RolledSectionHShape))
                {
                    var designService = SectionDesignExcelInteropService<RolledSectionHShape>.Instance;
                    currentUtilizationRatios = designService.Design(sections.OfType<RolledSectionHShape>(), designData, filePath);
                }
                else if (key == typeof(RolledSectionCShape))
                {
                    var designService = SectionDesignExcelInteropService<RolledSectionCShape>.Instance;
                    currentUtilizationRatios = designService.Design(sections.OfType<RolledSectionCShape>(), designData, filePath);
                }
                else if (key == typeof(RolledSectionLShape))
                {
                    var designService = SectionDesignExcelInteropService<RolledSectionLShape>.Instance;
                    currentUtilizationRatios = designService.Design(sections.OfType<RolledSectionLShape>(), designData, filePath);
                }
                else if (key == typeof(RolledSectionOShape))
                {
                    var designService = SectionDesignExcelInteropService<RolledSectionOShape>.Instance;
                    currentUtilizationRatios = designService.Design(sections.OfType<RolledSectionOShape>(), designData, filePath);
                }
                else
                {
                    var designService = SectionDesignExcelInteropService<RolledSectionCShape>.Instance;
                    currentUtilizationRatios = designService.Design(sections.OfType<RolledSectionCShape>(), designData, filePath);
                }

                allUtilizationRatios.Union(currentUtilizationRatios);
            }

            return allUtilizationRatios;
        }

        #endregion

        #region Private Helpers

        private static Dictionary<Type, List<IRolledSection>> SegregateSectionsByType(IEnumerable<IRolledSection> sections)
        {
            // Initialize the dictionary to store segregated sections
            Dictionary<Type, List<IRolledSection>> segregatedSections = new Dictionary<Type, List<IRolledSection>>();

            // Iterate through each section and segregate them by type
            foreach (var section in sections)
            {
                Type sectionType = section.GetType();

                // Check if the type already exists in the dictionary
                if (!segregatedSections.ContainsKey(sectionType))
                {
                    segregatedSections[sectionType] = new List<IRolledSection>();
                }

                // Add the section to the corresponding type's list
                segregatedSections[sectionType].Add(section);
            }

            return segregatedSections;
        }

        #endregion
    }
}
