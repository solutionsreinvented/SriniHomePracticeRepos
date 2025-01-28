using System.IO;

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

        public string JsonOutputFileFullPath => Path.Combine(TargetDirectory, $"{StaadFileName}_POReport.js");

        #endregion
    }
}
