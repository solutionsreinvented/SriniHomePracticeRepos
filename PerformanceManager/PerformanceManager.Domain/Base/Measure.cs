using PerformanceManager.Domain.Models;

using ReInvented.Shared.Stores;

using System;

namespace PerformanceManager.Domain.Base
{
    public class Measure : PropertyStore
    {
        public Measure()
        {
            CurrentDate = DateTime.Today;
        }


        public DateTime StartedOn { get => Get<DateTime>(); set => Set(value); }

        public Project Project { get => Get<Project>(); set => Set(value); }

        public DateTime CurrentDate { get;}
    }
}
