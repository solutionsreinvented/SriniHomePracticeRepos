using System;

using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.Domain.Design.ExcelInterop.Services
{
    public class ExcelTemplateNameProvider
    {
        public static string GetName(Type sectionType)
        {
            string fileName;

            if (sectionType == typeof(RolledSectionHShape))
            {
                fileName = "I";
            }
            else if(sectionType == typeof(RolledSectionCShape))
            {
                fileName = "C";
            }
            else if(sectionType == typeof(RolledSectionLShape))
            {
                fileName = "L";
            }
            else if(sectionType == typeof(RolledSectionOShape))
            {
                fileName = "O";
            }
            else
            {
                fileName = "Box";
            }

            return $"{fileName}Column.xlsm";
        }
    }
}
