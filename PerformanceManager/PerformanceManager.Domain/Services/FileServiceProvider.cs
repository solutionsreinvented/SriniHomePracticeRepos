namespace PerformanceManager.Domain.Services
{
    public static class FileServiceProvider
    {
        private static readonly string _dataDirectoryPath = @"C:\Users\masanams\source\SriniHomePracticeRepos\PerformanceManager\PerformanceManager.Domain\Data\";

        public static string GetDataDirectory() => _dataDirectoryPath;
    }
}
