using Newtonsoft.Json;

using ProdActivity.Domain.Models;

using System.IO;

namespace ProdActivity.Domain.Services
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
