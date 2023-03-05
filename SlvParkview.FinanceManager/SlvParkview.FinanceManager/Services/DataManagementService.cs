using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;
using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Models;

using System;
using System.IO;
using System.Windows;

namespace SlvParkview.FinanceManager.Services
{
    /// <summary>
    /// Provides the data retrieval and persistence services for an apartment block.
    /// </summary>
    public class DataManagementService : PropertyStore
    {
        #region Private Fields

        private string _filePath;
        private IDataSerializer<Block> _dataSerializer;

        private static DataManagementService _dataManagementService;

        #endregion

        #region Default Constructor

        private DataManagementService()
        {
            Initialize();
        }

        #endregion

        #region Singleton Instance

        public static DataManagementService Instance => _dataManagementService ?? (_dataManagementService = new DataManagementService());

        #endregion

        #region Read-only Properties

        public bool AllowSave { get => Get<bool>(); private set => Set(value); }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the existing data from a Json file.
        /// </summary>
        public Block RetrieveData()
        {
            return RetrieveData(_filePath);
        }

        public Block RetrieveData(string filePath)
        {
            _dataSerializer = new JsonDataSerializer<Block>();
            Block retrievedData = _dataSerializer.Deserialize(filePath);

            AllowSave = true;

            return retrievedData;
        }

        /// <summary>
        /// Persists the contents of the entire Block.
        /// </summary>
        public void SaveData(Block block)
        {
            SaveData(block, _filePath);
        }

        /// <summary>
        /// Persists the contents of the entire Block.
        /// </summary>
        public void SaveData(Block block, string filePath)
        {
            _dataSerializer = new JsonDataSerializer<Block>();

            block.LastUpdated = DateTime.Now;

            string serializedData = _dataSerializer.Serialize(block);

            try
            {
                File.WriteAllText(filePath, serializedData);
                _ = MessageBox.Show("Data saved successfully!", "Save data", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }
        }

        private void GenerateJsonFromCSV(string filePath)
        {

        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            _filePath = Path.Combine(ServiceProvider.AppDirectory, "C Block.json");
            _dataSerializer = new JsonDataSerializer<Block>();

            AllowSave = false;
        }

        #endregion

    }
}
