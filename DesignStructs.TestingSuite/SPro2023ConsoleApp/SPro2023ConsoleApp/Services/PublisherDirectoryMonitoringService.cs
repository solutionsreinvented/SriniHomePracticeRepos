using System;
using System.IO;
using System.Threading;

using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace SPro2023ConsoleApp.Services
{
    public class PublisherDirectoryMonitoringService
    {
        public static void Initiate(string filepath, bool includeSubdirectories = true)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(filepath)
            {
                // Set properties to watch for various types of changes
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes,
                IncludeSubdirectories = includeSubdirectories,
                EnableRaisingEvents = true
            };

            // Subscribe to the events you're interested in
            watcher.Created += OnFileCreated;
            watcher.Changed += OnFileChanged;
            watcher.Deleted += OnFileDeleted;
            watcher.Renamed += OnFileRenamed;

            //// Start watching the directory
            //watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the application
            Console.WriteLine("Press 'q' to quit the program.");
            while (Console.Read() != 'q') ;
        }

        // Event handlers for various types of changes
        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File created: {e.Name}");
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File changed: {e.Name}");
        }

        private static void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File deleted: {e.Name}");
        }

        private static void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"File renamed: {e.OldName} to {e.Name}");
        }
    }
}
