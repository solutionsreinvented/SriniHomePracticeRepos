using System.IO;
using System.Linq;

namespace TimCoreyFilesRenamer.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {

        }

        private void OnRenameFilesClicked()
        {
            string path = @"D:\02. Video Tutorials\02. Not Listed\Tim Corey - Course Collections (2020-2021)";


            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DirectoryInfo[]? rootDirectories = directoryInfo.GetDirectories();

            foreach (DirectoryInfo? dir in rootDirectories)
            {
                string targetDir = dir.FullName;

                FileInfo? contentsFile = dir.GetFiles("*.txt").First();
                FileInfo[]? mp4Files = dir.GetFiles("*.mp4");

                var names = File.ReadAllLines(contentsFile.FullName);

                foreach (var file in mp4Files)
                {
                    //if (file.Exists)
                    //{
                    //    file.CopyTo
                    //}
                }

            }

        }
    }
}
