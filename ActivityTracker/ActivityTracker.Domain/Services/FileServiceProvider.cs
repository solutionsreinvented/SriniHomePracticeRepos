using System;
using System.IO;

namespace ActivityTracker.Domain.Services
{
    public static class FileServiceProvider
    {
        private static readonly string _dataDirectoryPath = @"C:\Users\masanams\source\SriniHomePracticeRepos\ActivityTracker\ActivityTracker.Domain\Data\";
        private static readonly string _projectMasterFileName = "ProjectMaster";
        private static readonly string _extension = "json";


        public static string DataDirectory => _dataDirectoryPath;

        public static string BackupDirectory => Path.Combine(DataDirectory, "Backup");

        public static string ProjectMasterFilePath => Path.Combine(DataDirectory, $"{_projectMasterFileName}.{_extension}");

        public static string BackupFilePath
        {
            get
            {
                DateTime now = DateTime.Now;
                string backupFilename = $"{_projectMasterFileName}_{now.ToShortDateString().Replace("-", "").Replace(" ", "").Replace(":", "")}_{now.ToLongTimeString().Replace(":", "")}.{_extension}";
                return Path.Combine(BackupDirectory, backupFilename);
            }
        }


        public static string ActivityMasterFilePath => Path.Combine(DataDirectory, $"ActivityCategories.{_extension}");

    }
}
