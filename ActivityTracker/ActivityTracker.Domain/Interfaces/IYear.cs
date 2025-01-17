using ProdActivity.Domain.Base;

using System;

namespace ProdActivity.Domain.Interfaces
{
    public interface IYear
    {
        DateTime StartDate { get; }

        DateTime EndDate { get; }

        CategorizedDays CategorizedDays { get; }
    }
}