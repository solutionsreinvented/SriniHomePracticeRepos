using System;

using ReInvented.Domain.Reporting.Models;

namespace ReInvented.Domain.Reporting.Interfaces
{
    public interface IReportContent
    {
        string GeneratedOn { get; }

        DataSourceInformation DataSourceInformation { get; }

    }
}
