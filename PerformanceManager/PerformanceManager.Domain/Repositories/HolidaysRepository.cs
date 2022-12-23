using System;
using System.Collections.Generic;
using System.IO;

using PerformanceManager.Domain.Services;

using SRi.XamlUIThickenerApp.DataAccess;

namespace PerformanceManager.Domain.Repositories
{
    public class HolidaysRepository
    {
        private const string _fileName = @"Holidays.json";

        private readonly JsonDataSerializer<HashSet<DateTime>> _jsonDataSerializer = new();

        public HolidaysRepository()
        {

        }

        public HashSet<DateTime> GetAll()
        {
            string fileFullPath = Path.Combine(FileServiceProvider.DataDirectory, _fileName);

            HashSet<DateTime> holidays = _jsonDataSerializer.Deserialiaze(fileFullPath);

            return holidays;

        }
    }
}
