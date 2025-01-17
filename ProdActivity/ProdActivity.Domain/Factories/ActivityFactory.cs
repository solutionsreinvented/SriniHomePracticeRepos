using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Models;

using System;

namespace ProdActivity.Domain.Factories
{
    public static class ActivityFactory
    {
        public static IActivity Create(Discipline domain, IProject selectedProject)
        {
            return domain switch
            {
                Discipline.Design => new DesignActivity(selectedProject),
                Discipline.Detailing => new DetailingActivity(selectedProject),
                Discipline.Development => new DevelopmentActivity(selectedProject),
                _ => throw new NotImplementedException($"The specified {domain} activity domain not found!"),
            };
        }
    }
}
