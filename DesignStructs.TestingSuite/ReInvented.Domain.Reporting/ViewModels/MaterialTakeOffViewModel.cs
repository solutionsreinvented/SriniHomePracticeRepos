using System.ComponentModel;
using System.IO;
using System.Windows;

using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;

namespace ReInvented.Domain.Reporting.ViewModels
{
    public class MaterialTakeOffViewModel : ReportViewModel, INotifyPropertyChanged
    {
        #region Default Constructor

        public MaterialTakeOffViewModel()
        {
            Report = new Report<MaterialTakeOff>();
        }

        #endregion

        #region Public Properties

        public Report<MaterialTakeOff> Report { get; set; }

        #endregion

        #region Event Handlers

        protected override void OnGenerateReport()
        {
            base.OnGenerateReport();

            Report.Content = MaterialTakeOffService.Generate(Wrapper);

            if (Report.Content != null)
            {
                bool useAbsolutePaths = true;

                DirectoryInfo projectDirectory = Directory.CreateDirectory(Path.Combine(ProjectInfo.ProjectDirectory, ProjectInfo.Code));

                MaterialTakeOffService.CreateReportHtmlFile(projectDirectory, useAbsolutePaths);

                if (!useAbsolutePaths)
                {
                    MaterialTakeOffService.CopyCssStyleFiles(projectDirectory);
                    MaterialTakeOffService.CopyJavaScriptFiles(projectDirectory);
                }

                MaterialTakeOffService.CreateReportContentsFile(projectDirectory, Report);
            }
            else
            {
                _ = MessageBox.Show("It appears that the report contents are empty. Make sure to generate the report contents first!", "Material Take Off", MessageBoxButton.OK);
            }
        }

        protected override void OnSelectRevisionHistoryFile()
        {
            FileFilter filter = new FileFilter("Revision History Files", FileExtensions.RevisionHistoryFile);

            Report.DocumentInfo.RevisionHistoryFilePath = FileServiceProvider.GetFilePathUsingOpenFileDialog(filter);
        }

        #endregion
    }
}
