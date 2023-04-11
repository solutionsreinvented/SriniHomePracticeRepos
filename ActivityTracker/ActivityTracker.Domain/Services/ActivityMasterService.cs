using Newtonsoft.Json;

using ActivityTracker.Domain.Models;

using System.IO;

namespace ActivityTracker.Domain.Services
{
    public static class ActivityMasterService
    {
        public static ActivityMaster ReadFromFile()
        {
            string fileContents = File.ReadAllText(FileServiceProvider.ActivityMasterFilePath);

            ActivityMaster activityMaster = JsonConvert.DeserializeObject<ActivityMaster>(fileContents);

            return activityMaster;
        }
    }
}
