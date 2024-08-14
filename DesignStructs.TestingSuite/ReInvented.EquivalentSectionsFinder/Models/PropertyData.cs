namespace ReInvented.EquivalentSectionsFinder.Models
{
    public sealed class PropertyData
    {
        #region Public Properties

        public string PropertyName { get; set; }

        public double SourceValue { get; set; }

        public double TargetValue { get; set; }

        public double PercentDifference { get; set; }

        #endregion
    }
}
