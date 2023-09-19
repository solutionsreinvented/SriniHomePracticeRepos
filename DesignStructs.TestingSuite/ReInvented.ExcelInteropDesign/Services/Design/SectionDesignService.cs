using System.Collections.Generic;
using System.IO;
using System.Linq;

using ReInvented.DataAccess.Services;
using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.ExcelInteropDesign.Models;
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

            Dictionary<RolledSectionKey, List<IRolledSection>> segregatedSections = SegregateSectionsByType(classifications.SelectMany(c => c.Sections).ToList());

            /// Check if the Axial Strength Parameters and Shear Strength Parameters are provided completely before starting the design process.
            if (segregatedSections.Keys.Any(k => !designData.AxialStrengthParametersDictionary.ContainsKey(k)) ||
                designData.AxialStrengthParametersDictionary.Values.Any(v => v == null))
            {
                throw new InvalidDataException($"Inadequate information is provided in {nameof(designData.AxialStrengthParametersDictionary)}");
            }

            if (segregatedSections.Keys.Any(k => !designData.ShearStrengthParametersDictionary.ContainsKey(k)) ||
                designData.ShearStrengthParametersDictionary.Values.Any(v => v == null))
            {
                throw new InvalidDataException($"Inadequate information is provided in {nameof(designData.ShearStrengthParametersDictionary)}");
            }


            foreach (RolledSectionKey key in segregatedSections.Keys)
            {
                filePath = Path.Combine(FileServiceProvider.ExcelTemplatesTestingDirectory, "Columns", ExcelTemplateNameProvider.GetName(key.KeyType));
                List<IRolledSection> sections = segregatedSections[key];

                Dictionary<string, double> currentUtilizationRatios;

                if (key.KeyType == typeof(RolledSectionHShape))
                {
                    var designService = SectionDesignExcelInteropService<RolledSectionHShape>.Instance;
                    currentUtilizationRatios = designService.Design(sections.OfType<RolledSectionHShape>(), designData, filePath);
                }
                else if (key.KeyType == typeof(RolledSectionCShape))
                {
                    var designService = SectionDesignExcelInteropService<RolledSectionCShape>.Instance;
                    currentUtilizationRatios = designService.Design(sections.OfType<RolledSectionCShape>(), designData, filePath);
                }
                else if (key.KeyType == typeof(RolledSectionLShape))
                {
                    var designService = SectionDesignExcelInteropService<RolledSectionLShape>.Instance;
                    currentUtilizationRatios = designService.Design(sections.OfType<RolledSectionLShape>(), designData, filePath);
                }
                else if (key.KeyType == typeof(RolledSectionOShape))
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

        private static Dictionary<RolledSectionKey, List<IRolledSection>> SegregateSectionsByType(IEnumerable<IRolledSection> sections)
        {
            // Initialize the dictionary to store segregated sections
            Dictionary<RolledSectionKey, List<IRolledSection>> segregatedSections = new Dictionary<RolledSectionKey, List<IRolledSection>>();

            // Iterate through each section and segregate them by type
            foreach (IRolledSection section in sections)
            {
                RolledSectionKey sectionKey = new RolledSectionKey(section.GetType());

                // Check if the type already exists in the dictionary
                if (!segregatedSections.ContainsKey(sectionKey))
                {
                    segregatedSections[sectionKey] = new List<IRolledSection>();
                }

                // Add the section to the corresponding type's list
                segregatedSections[sectionKey].Add(section);
            }

            return segregatedSections;
        }

        #endregion
    }
}
