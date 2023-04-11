using ActivityTracker.Domain.Base;

using System;

namespace ActivityTracker.Domain.Interfaces
{
    public interface IYear
    {
        DateTime StartDate { get; }

        DateTime EndDate { get; }

        CategorizedDays CategorizedDays { get; }
    }
}