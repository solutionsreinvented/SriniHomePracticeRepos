using System.IO;

using ReInvented.DataAccess.Services;

namespace ReInvented.Domain.Optimization.Models
{
    public sealed class OutputFiles
    {
        #region Default Constructor

        public OutputFiles()
        {

        }

        #endregion

        #region Parameterized Constructor

        public OutputFiles(string staadFileFullPath)
        {
            StaadFileFullPath = staadFileFullPath;
        }

        #endregion

        #region Public Properties

        public string StaadFileFullPath { get; set; }

        public string TargetDirectory => Path.GetDirectoryName(StaadFileFullPath);

        public string StaadFileName => Path.GetFileNameWithoutExtension(StaadFileFullPath);

        public string ReportDataFileJson => Path.Combine(TargetDirectory, $"{StaadFileName}_POReportData.{FileExtensions.Json}");

        public string ReportHtmlFile => Path.Combine(TargetDirectory, $"{StaadFileName}_POReport.{FileExtensions.Html}");

        #endregion
    }
}
