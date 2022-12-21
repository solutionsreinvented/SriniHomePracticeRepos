using PerformanceManager.Domain.Base;

using System;

namespace PerformanceManager.Domain.Interfaces
{
    public interface IYear
    {
        DateTime StartDate { get; }

        DateTime EndDate { get; }

        CategorizedDays CategorizedDays { get; }
    }
}