using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;

///using Microsoft.Win32;

using StockAnalyzer.Commands;
using StockAnalyzer.Enums;
using StockAnalyzer.Models;
using StockAnalyzer.Services;

namespace StockAnalyzer.ViewModels
{
    public class ScripsProcessingViewModel : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Fields

        private string _filePath;
        private decimal _minimumValue;
        private decimal _maximumValue;
        private int _numberOfScrips;
        private string _dataStatus;
        private Scrip _selectedScrip;
        private bool _showCircuitStocksOnly;
        private ScripFilter _scripFilter;

        #endregion

        #region Default Constructor

        public ScripsProcessingViewModel()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(); OnProcessScrips();
            }
        }

        public decimal MinimumValue
        {
            get => _minimumValue;
            set
            {
                _minimumValue = value;
                OnPropertyChanged(); OnProcessScrips();
            }
        }

        public decimal MaximumValue
        {
            get => _maximumValue;
            set
            {
                _maximumValue = value;
                OnPropertyChanged(); OnProcessScrips();
            }
        }

        public int NumberOfScrips
        {
            get => _numberOfScrips;
            set
            {
                _numberOfScrips = value;
                OnPropertyChanged(); OnProcessScrips();
            }
        }

        public string DataStatus
        {
            get => _dataStatus;
            set
            {
                _dataStatus = value;
                OnPropertyChanged();
            }
        }

        public bool ShowCircuitStocksOnly
        {
            get => _showCircuitStocksOnly;
            set
            {
                _showCircuitStocksOnly = value;
                OnPropertyChanged();
            }
        }

        public ScripFilter ScripFilter
        {
            get => _scripFilter;
            set
            {
                _scripFilter = value;
                OnPropertyChanged();
                OnProcessScrips();
            }
        }

        public IEnumerable<Scrip> ScripsInRange { get; set; }

        public Scrip SelectedScrip
        {
            get => _selectedScrip;
            set
            {
                _selectedScrip = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand GetFilePathCommand { get; set; }

        public ICommand ProcessScripsCommand { get; set; }

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
        private void OnProcessScrips()
        {
            ScripsInRange = new List<Scrip>();

            if (File.Exists(FilePath))
            {
                if (DataIsValid())
                {

                    ScripsInRange = StockDataProcessor.GetScrips(FilePath)
                                                      .Where(s => s.LastTraded >= MinimumValue && s.LastTraded <= MaximumValue)
                                                      .Take(NumberOfScrips).OrderBy(s => s.LastTraded);

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

        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void Initialize()
        {
            FilePath = @"C:\\Users\\masanams\\Desktop\\EQ" + $"{DateTime.Today:ddMMyy}.CSV";
            MinimumValue = 100;
            MaximumValue = 150;
            NumberOfScrips = 15;
            ShowCircuitStocksOnly = false;
            ScripFilter = ScripFilter.LastTraded;

            GetFilePathCommand = new RelayCommand(OnGetFilePath, true);
            ProcessScripsCommand = new RelayCommand(OnProcessScrips, true);
        }

        private bool CircuitOnly(Scrip scrip)
        {
            return scrip.High == scrip.Low && scrip.High == scrip.Open && scrip.High == scrip.Close;
        }


        #endregion

    }
}
