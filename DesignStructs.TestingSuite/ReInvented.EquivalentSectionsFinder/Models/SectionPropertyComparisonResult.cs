using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared;

namespace Continuum.EquivalentSectionsFinder.Models
{
    public sealed class SectionEquivalency
    {
        #region Public Properties

        public string SourceName { get; set; }
        public string TargetName { get; set; }
        public int ValidValues { get; set; }
        public int MatchedValues { get; set; }
        public double MatchPercent { get; set; }

        #endregion

        #region Static Functions

        public static SectionEquivalency GetEquivalency(Dictionary<string, double> results, string sourceName, string targetName, double percentDifference)
        {
            IEnumerable<double> validValues = results.Values.Where(v => !double.IsNaN(v));
            IEnumerable<double> matchedValues = validValues.Where(value => IsInRange(value, percentDifference));
            double matchPercent = matchedValues.Count() / (double)validValues.Count();

            return new SectionEquivalency() { SourceName = sourceName, TargetName = targetName, ValidValues = validValues.Count(), MatchedValues = matchedValues.Count(), MatchPercent = matchPercent };
        } 

        #endregion

        #region Private Helpers

        private static bool IsInRange(double value, double perecentDifference)
        {
            return value >= 0 && value <= perecentDifference;
        }

        #endregion

    }

    public sealed class SectionPropertyComparisonResult
    {
        #region Parameterized Constructor

        public SectionPropertyComparisonResult(IRolledSection sourceSection, IRolledSection targetSection)
        {
            Source = sourceSection;
            Target = targetSection;
            Results = Source.GetPercentDifferences(Target);
        }

        #endregion

        #region Public Properties

        public IRolledSection Source { get; set; }

        public IRolledSection Target { get; set; }

        public Dictionary<string, double> Results { get; private set; }

        public SectionEquivalency SectionEquivalency { get; private set; }

        #endregion

        #region Public Functions

        public bool IsEquivalent(double percentDifference, double matchProbabilityPercent)
        {
            //IEnumerable<double> validValues = Results.Values.Where(v => !double.IsNaN(v));
            //IEnumerable<double> matchedValues = validValues.Where(value => IsInRange(value, percentDifference));
            //double matchPercent = matchedValues.Count() / (double)validValues.Count();

            //SectionEquivalency = new SectionEquivalency() { SourceName = Source.Designation, TargetName = Target.Designation, ValidValues = validValues.Count(), MatchedValues = matchedValues.Count(), MatchPercent = matchPercent };

            SectionEquivalency = SectionEquivalency.GetEquivalency(Results, Source.Designation, Target.Designation, percentDifference);

            return SectionEquivalency.MatchPercent.InPercentage() >= matchProbabilityPercent.InPercentage(); /// matchPercent.InPercentage() >= matchProbabilityPercent.InPercentage();
        }

        #endregion



        #region Helper Properties

        public IEnumerable<PropertyData> ProjectedProperties
        {
            get
            {
                List<PropertyData> projectedProperties = new();
                Type targetType = Target.GetType();

                PropertyData dbPropertyData = GenerateDatabasePropertyData();
                projectedProperties.Add(dbPropertyData);

                foreach (KeyValuePair<string, double> kvp in Results)
                {
                    PropertyData propertyData = GenerateEachPropertyData(targetType, kvp);
                    projectedProperties.Add(propertyData);
                }


                return projectedProperties;
            }
        }

        private PropertyData GenerateEachPropertyData(Type targetType, KeyValuePair<string, double> kvp)
        {
            PropertyData propertyData = new() { PropertyName = kvp.Key, PercentDifference = kvp.Value.ToString("P2") };
            PropertyInfo propertyInfo = targetType.GetProperty(kvp.Key);

            if (propertyInfo != null && propertyInfo.PropertyType == typeof(double))
            {
                double sourceValue = (double)propertyInfo.GetValue(Source);
                double targetValue = (double)propertyInfo.GetValue(Target);

                propertyData.SourceValue = sourceValue.ToString("N2");
                propertyData.TargetValue = targetValue.ToString("N2");
            }

            return propertyData;
        }

        private PropertyData GenerateDatabasePropertyData()
        {
            PropertyData dbPropertyData = new() { PropertyName = "Database" };
            dbPropertyData.SourceValue = SectionsRepository.Instance.GetSectionsLibrary().GetDatabaseOf(Source).Name;
            dbPropertyData.TargetValue = SectionsRepository.Instance.GetSectionsLibrary().GetDatabaseOf(Target).Name;
            dbPropertyData.PercentDifference = "NA";
            return dbPropertyData;
        }

        #endregion
    }
}
