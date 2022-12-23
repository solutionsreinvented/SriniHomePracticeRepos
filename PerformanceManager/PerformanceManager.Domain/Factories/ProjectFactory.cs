using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;

using System;

namespace PerformanceManager.Domain.Factories
{
    public static class ProjectFactory
    {
        public static IProject Create(ProjectType type)
        {
            return type switch
            {
                ProjectType.PreOrder => new PreOrder(),
                ProjectType.Order => new Order(),
                ProjectType.Development => new Development(),
                _ => throw new NotImplementedException("Specified project type is not found!"),
            };
        }
    }
}
