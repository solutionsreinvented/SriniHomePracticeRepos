using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Shared;

namespace ReInvented.EquivalentSectionsFinder.Models
{
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

        #endregion

        #region Public Functions

        public bool IsEquivalent(double percentDifference, double matchProbabilityPercent)
        {
            IEnumerable<double> validValues = Results.Values.Where(v => v != double.NaN);
            IEnumerable<double> matchedValues = validValues.Where(value => IsInRange(value, percentDifference));
            double matchPercent = matchedValues.Count() / (double)validValues.Count();

            return matchPercent.InPercentage() >= matchProbabilityPercent.InPercentage();
        }

        #endregion

        #region Private Helpers

        private bool IsInRange(double value, double perecentDifference)
        {
            return value >= 0 && value <= perecentDifference;
        }

        #endregion

        #region Helper Properties

        public IEnumerable<PropertyData> ProjectedProperties
        {
            get
            {
                List<PropertyData> projectedProperties = new();
                Type targetType = Target.GetType();

                foreach (KeyValuePair<string, double> kvp in Results)
                {
                    PropertyData propertyData = new PropertyData() { PropertyName = kvp.Key, PercentDifference = kvp.Value };
                    PropertyInfo propertyInfo = targetType.GetProperty(kvp.Key);

                    if (propertyInfo != null && propertyInfo.PropertyType == typeof(double))
                    {
                        double sourceValue = (double)propertyInfo.GetValue(Source);
                        double targetValue = (double)propertyInfo.GetValue(Target);

                        propertyData.SourceValue = sourceValue;
                        propertyData.TargetValue = targetValue;
                    }

                    projectedProperties.Add(propertyData);
                }

                return projectedProperties;
            }
        }

        #endregion
    }
}
