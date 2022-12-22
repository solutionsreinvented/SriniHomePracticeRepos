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
    public class ResourceRepository
    {
        private const string _fileName = @"Resources.json";

        private readonly JsonDataSerializer<List<Resource>> _jsonDataSerializer = new();

        public ResourceRepository()
        {

        }

        public List<IResource> GetAll()
        {
            string fileFullPath = Path.Combine(FileServiceProvider.GetDataDirectory(), _fileName);

            List<Resource> resources = _jsonDataSerializer.Deserialiaze(fileFullPath);

            resources.First().Activities = new HashSet<IActivity>()
            {
                new Activity()
                {
                    ///ActivityType = ActivityType.Detailing,
                    Description = "New Activity",
                    InitiatedOn = DateTime.Now
                }
            };

            return ClassToInterfaceMapper<Resource, IResource>.Map(resources);

        }

    }
}
