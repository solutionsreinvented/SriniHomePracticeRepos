using Newtonsoft.Json;

using PerformanceManager.Domain.Models;

using System.IO;

namespace PerformanceManager.Domain.Services
{
    public static class ActivityMasterService
    {
        public static ActivityMaster ReadFromFile()
        {
            string fileContents = File.ReadAllText(Path.Combine(FileServiceProvider.DataDirectory, "ActivityCategories.json"));

            ActivityMaster activityMaster = JsonConvert.DeserializeObject<ActivityMaster>(fileContents);

            return activityMaster;
        }
    }
}
