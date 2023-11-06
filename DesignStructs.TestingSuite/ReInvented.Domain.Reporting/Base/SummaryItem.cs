namespace ReInvented.Domain.Reporting.Models.Base
{
    public abstract class SummaryItem
    {
        #region Public Properties

        public int SlNo { get; set; }

        public string Description { get; set; }

        public string AssemblyGroup { get; set; }

        public double Weight { get; set; }

        #endregion
    }
}
