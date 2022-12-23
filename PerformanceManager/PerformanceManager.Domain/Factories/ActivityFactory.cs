using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;

using System;

namespace PerformanceManager.Domain.Factories
{
    public static class ActivityFactory
    {
        public static IActivity Create(Discipline domain)
        {
            return domain switch
            {
                Discipline.Design => new DesignActivity(),
                Discipline.Detailing => new DetailingActivity(),
                Discipline.Development => new DevelopmentActivity(),
                _ => throw new NotImplementedException($"The specified {domain} activity domain not found!"),
            };
        }
    }
}
