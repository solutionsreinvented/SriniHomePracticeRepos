using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ReInvented.Validation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string folderPath = @"D:\";
            string searchPattern = "*.xls*"; // Specify the search pattern

            string[] exclusionKeywords = { "shear", "completed" }; // Specify exclusion keywords

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            FileInfo[] files = directoryInfo.GetFiles(searchPattern);

            var filteredFiles = files
                .Where(file => !exclusionKeywords.Any(keyword => file.Name.Contains(keyword)))
                .ToArray();

            Console.WriteLine("Files found:");
            foreach (var file in filteredFiles)
            {
                Console.WriteLine(file.Name);
            }
        }
    }
}
