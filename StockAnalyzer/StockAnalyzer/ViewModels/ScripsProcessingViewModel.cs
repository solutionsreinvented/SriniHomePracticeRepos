using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

using ReInvented.Shared.Stores;

///using Microsoft.Win32;

using StockAnalyzer.Commands;
using StockAnalyzer.Enums;
using StockAnalyzer.Models;
using StockAnalyzer.Services;

namespace StockAnalyzer.ViewModels
{
    public class ScripsProcessingViewModel : PropertyStore
    {
        #region Default Constructor

        public ScripsProcessingViewModel()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public string FilePath { get => Get<string>(); set { Set(value); ProcessScrips(); } }

        public decimal MinimumValue { get => Get<decimal>(); set { Set(value); ProcessScrips(); } }

        public decimal MaximumValue { get => Get<decimal>(); set { Set(value); ProcessScrips(); } }

        public int NumberOfScrips { get => Get<int>(); set { Set(value); ProcessScrips(); } }

        public string DataStatus { get => Get<string>(); set => Set(value); }

        public bool CircuitStocksOnly { get => Get<bool>(); set { Set(value); ProcessScrips(); } }

        public ScripFilter ScripFilter { get => Get<ScripFilter>(); set { Set(value); ProcessScrips(); } }

        public Scrip SelectedScrip { get => Get<Scrip>(); set => Set(value); }

        public IEnumerable<Scrip> ScripsInRange { get => Get<IEnumerable<Scrip>>(); private set => Set(value); }

        #endregion

        #region Commands

        public ICommand GetFilePathCommand { get; set; }

        //public ICommand ProcessScripsCommand { get; set; }

        #endregion

        #region Private Methods

        private void OnGetFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "*Csv files (*.csv)|*.csv",
                Multiselect = false,
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = openFileDialog.FileName;
            }
        }
        private void ProcessScrips()
        {
            ScripsInRange = new List<Scrip>();

            PropertyInfo filterPropInfo = typeof(Scrip).GetProperty(ScripFilter.ToString());

            if (File.Exists(FilePath))
            {
                if (DataIsValid())
                {

                    ScripsInRange = (from scrip in StockDataProcessor.GetScrips(FilePath)
                                     let propertyValue = (decimal)filterPropInfo.GetValue(scrip)
                                     where propertyValue >= MinimumValue && propertyValue <= MaximumValue
                                     orderby propertyValue
                                     select scrip);

                    if (CircuitStocksOnly)
                    {
                        ScripsInRange = ScripsInRange?.Where(s => s.LastTraded == s.Open && s.LastTraded == s.High && s.LastTraded == s.Low && s.LastTraded == s.Close);
                    }

                    ScripsInRange = ScripsInRange.Take(NumberOfScrips);

                    DataStatus = "Check out the results below";
                }
            }
            else
            {
                DataStatus = "Unable to locate the specified file. Either the file does not exists or you haven't selected one.";
            }

            OnPropertyChanged(nameof(ScripsInRange));
        }

        #region Data Is Valid Method

        private bool DataIsValid()
        {
            bool dataIsValid = true;

            if (MinimumValue < 0.0m)
            {
                DataStatus = "Minimum value shall be greater than or equal to zero.";
                dataIsValid = false;
            }
            else if (MaximumValue < 0.0m)
            {
                DataStatus = "Maximum value shall be greater than or equal to zero.";
                dataIsValid = false;
            }
            else if (MinimumValue >= MaximumValue)
            {
                DataStatus = "Minimum value shall be less than maximum value.";
                dataIsValid = false;
            }
            else if (NumberOfScrips <= 0)
            {
                DataStatus = "Number of scrips shall be greater than or equal 1.";
                dataIsValid = false;
            }
            else
            {
                DataStatus = "";
            }

            return dataIsValid;
        }

        #endregion

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            FilePath = @"C:\\Users\\masanams\\Desktop\\EQ" + $"{DateTime.Today:ddMMyy}.CSV";
            MinimumValue = 100;
            MaximumValue = 150;
            NumberOfScrips = 15;
            CircuitStocksOnly = false;
            ScripFilter = ScripFilter.LastTraded;

            GetFilePathCommand = new RelayCommand(OnGetFilePath, true);
        }

        #endregion

    }
}
