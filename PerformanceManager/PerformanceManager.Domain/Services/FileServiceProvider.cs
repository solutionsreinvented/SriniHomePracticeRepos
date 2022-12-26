using System.IO;

namespace PerformanceManager.Domain.Services
{
    public static class FileServiceProvider
    {
        private static readonly string _dataDirectoryPath = @"C:\Users\masanams\source\SriniHomePracticeRepos\PerformanceManager\PerformanceManager.Domain\Data\";

        public static string DataDirectory => _dataDirectoryPath;

        public static string ProjectMasterFilePath => Path.Combine(DataDirectory, "ProjectMaster.json");
    }
}
