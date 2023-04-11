using System.IO;

namespace ActivityTracker.Domain.Services
{
    public static class FileServiceProvider
    {
        private static readonly string _dataDirectoryPath = @"C:\Users\masanams\source\SriniHomePracticeRepos\ActivityTracker\ActivityTracker.Domain\Data\";

        public static string DataDirectory => _dataDirectoryPath;

        public static string ProjectMasterFilePath => Path.Combine(DataDirectory, "ProjectMaster.json");

        public static string ActivityMasterFilePath => Path.Combine(DataDirectory, "ActivityCategories.json");

    }
}
