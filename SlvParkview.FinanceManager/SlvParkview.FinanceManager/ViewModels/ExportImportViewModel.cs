using System.Windows.Forms;
using System.Windows.Input;

using ReInvented.Shared.Commands;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ExportImportViewModel : BaseViewModel
    {
        #region Default Constructor

        public ExportImportViewModel()
        {
            InitializeCommands();
        }

        #endregion

        #region Public Properties

        public string SourcePath { get => Get<string>(); set => Set(value); }

        public string DestinationPath { get => Get<string>(); set => Set(value); }

        public Tower SelectedTower { get => Get<Tower>(); set => Set(value); }

        #endregion

        #region Commands

        public ICommand SelectSource { get; set; }

        public ICommand SelectDestination { get; set; }

        public ICommand ExportToTarget { get; set; }

        #endregion

        #region Command Handlers

        private void OnSelectSource()
        {
            SourcePath = GetPathUsingOpenFileDialog("xlsx", "Excel");
        }

        private void OnSelectDestination()
        {
            DestinationPath = GetPathUsingSaveFileDialog("json", "Json");
        }

        private void OnExportToTarget()
        {
            ExcelExportImportService.ExportFlatDataFromExcelToJson(SourcePath, DestinationPath, SelectedTower.ToString());
            _ = MessageBox.Show("Data successfully exported to the designation!!!", "Data export", MessageBoxButtons.OK);
        }

        #endregion

        #region Private Helpers

        private void InitializeCommands()
        {
            SelectedTower = Tower.A;
            SelectSource = new RelayCommand(OnSelectSource, true);
            SelectDestination = new RelayCommand(OnSelectDestination, true);
            ExportToTarget = new RelayCommand(OnExportToTarget, true);
        }

        private string GetPathUsingOpenFileDialog(string filter, string filesDescription)
        {
            string filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = $"{filesDescription} Files (*.{filter})|*.{filter}",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }

            openFileDialog.Dispose();

            return filePath;
        }

        private string GetPathUsingSaveFileDialog(string filter, string filesDescription)
        {
            string filePath = string.Empty;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = $"{filesDescription} Files (*.{filter})|*.{filter}",
                Title = "Select the destination path",
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = saveFileDialog.FileName;
            }

            saveFileDialog.Dispose();

            return filePath;
        }

        #endregion

    }
}
