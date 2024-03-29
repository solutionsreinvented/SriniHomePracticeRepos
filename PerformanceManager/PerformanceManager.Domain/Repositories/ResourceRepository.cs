﻿using System.Collections.Generic;
using System.IO;

using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Mappers;
using PerformanceManager.Domain.Services;

using ReInvented.DataAccess;

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
            string fileFullPath = Path.Combine(FileServiceProvider.DataDirectory, _fileName);

            List<Resource> resources = _jsonDataSerializer.Deserialize(fileFullPath);

            //resources.First().Activities = new HashSet<IActivity>()
            //{
            //    new Activity()
            //    {
            //        InitiatedOn = DateTime.Now
            //    }
            //};

            return ClassToInterfaceMapper<Resource, IResource>.Map(resources);

        }

    }
}
