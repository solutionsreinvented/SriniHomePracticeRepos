using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Interfaces;

namespace ReInvented.Domain.Reporting.Models
{
    public class MTOReport : Report, IReport
    {
        #region Default Constructor

        public MTOReport()
        {
            Contingencies = new Contingencies() { Connections = 0.12, Plates = 0.05, Sections = 0.12 };
        }

        #endregion

        #region Public Properties

        public Contingencies Contingencies { get => Get<Contingencies>(); private set => Set(value); }

        #endregion

    }
}
