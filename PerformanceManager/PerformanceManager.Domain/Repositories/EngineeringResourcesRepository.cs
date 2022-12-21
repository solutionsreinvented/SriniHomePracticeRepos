using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Mappers;
using PerformanceManager.Domain.Services;

using SRi.XamlUIThickenerApp.DataAccess;

namespace PerformanceManager.Domain.Repositories
{
    public class EngineeringResourcesRepository
    {
        private const string _fileName = @"EngineeringResources.json";

        private readonly JsonDataSerializer<List<EngineeringResource>> _jsonDataSerializer = new();

        public EngineeringResourcesRepository()
        {

        }

        public List<IEngineeringResource> GetAll()
        {
            string fileFullPath = Path.Combine(FileServiceProvider.GetDataDirectory(), _fileName);

            List<EngineeringResource> resources = _jsonDataSerializer.Deserialiaze(fileFullPath);

            resources.First().Activities = new List<IEngineeringActivity>()
            {
                new EngineeringActivity()
                {
                    ActivityType = EngineeringActivityType.Drafting,
                    Description = "New Activity",
                    InitiatedOn = DateTime.Now
                }
            };

            return ClassToInterfaceMapper<EngineeringResource, IEngineeringResource>.Map(resources);

        }

    }
}
