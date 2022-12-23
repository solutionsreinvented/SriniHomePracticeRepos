using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;

namespace PerformanceManager.Domain.Services
{
    public static class HolidaysService
    {
        public static HashSet<DateTime> GetAllHolidays()
        {
            string fileContents = File.ReadAllText(Path.Combine(FileServiceProvider.DataDirectory, "Holidays.json"));
            HashSet<DateTime> holidays = JsonConvert.DeserializeObject<HashSet<DateTime>>(fileContents);

            return holidays;
        }
    }
}
