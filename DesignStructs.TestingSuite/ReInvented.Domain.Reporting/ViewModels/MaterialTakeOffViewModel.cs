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

            MaterialTakeOff mto = MaterialTakeOffService.Generate(,)


            if (foundationLoadData != null)
            {
                bool useAbsolutePaths = true;

                DirectoryInfo projectDirectory = Directory.CreateDirectory(Path.Combine(ProjectInfo.ProjectDirectory, ProjectInfo.Code));

                FoundationLoadDataService.CreateReportHtmlFile(projectDirectory, useAbsolutePaths);

                if (!useAbsolutePaths)
                {
                    FoundationLoadDataService.CopyCssStyleFiles(projectDirectory);
                    FoundationLoadDataService.CopyJavaScriptFiles(projectDirectory);
                }

                FoundationLoadDataService.CreateReportContentsFile(projectDirectory, foundationLoadData);
            }
            else
            {
                _ = MessageBox.Show("It appears that the analysis results are not available. Make sure to run the analysis before generating foundation load data!", "Foundation Load Data", MessageBoxButton.OK);
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
