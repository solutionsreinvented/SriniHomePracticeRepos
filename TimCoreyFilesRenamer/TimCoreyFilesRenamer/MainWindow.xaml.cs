using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace TimCoreyFilesRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Helpers
        private static string NameWithoutExtension(FileInfo file)
        {
            return file.Name.Replace(".mp4", string.Empty);
        }

        private static void StripLessonFromNames(FileInfo[] files)
        {
            foreach (var file in files)
            {
                if (file.Exists & file.Name.Contains("Lesson"))
                {
                    string newName = file.FullName.Replace("Lesson", string.Empty);
                    file.MoveTo(newName, false);
                }
            }
        } 
        #endregion

        private void OnRenameFilesClicked(object sender, RoutedEventArgs e)
        {
            string path = @"D:\02. Video Tutorials\02. Not Listed\Tim Corey - Course Collections (2020-2021)";


            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DirectoryInfo[]? rootDirectories = directoryInfo.GetDirectories();

            foreach (DirectoryInfo? dir in rootDirectories)
            {
                ///string targetDir = dir.FullName;

                FileInfo? contentsFile = dir.GetFiles("*.txt").First();

                FileInfo[]? mp4Files = dir.GetFiles("*.mp4");
                string[]? names = File.ReadAllLines(contentsFile.FullName);

                StripLessonFromNames(mp4Files);
                AddDescriptionToNames(mp4Files, names);
            }
        }

        private static void AddDescriptionToNames(FileInfo[] mp4Files, string[] names)
        {
            List<FileInfo>? orderedFiles = mp4Files.OrderBy(f => int.Parse(NameWithoutExtension(f))).ToList();
            string? targetDir = mp4Files.First().Directory.FullName;

            for (int i = 0; i < orderedFiles.Count; i++)
            {
                FileInfo? file = orderedFiles[i];

                if (file.Exists)
                {
                    string newName = Path.Combine(targetDir, $"{NameWithoutExtension(file)}. {names[i].Trim()}.mp4");
                    file.MoveTo(newName);
                }
            }
        }
    }
}
