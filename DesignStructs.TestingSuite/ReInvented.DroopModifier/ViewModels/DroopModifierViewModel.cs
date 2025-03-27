using System;

using ReInvented.DroopModifier.Models;
using ReInvented.ExcelInterop.Services;
using ReInvented.Shared.Stores;
using Microsoft.Office.Interop.Excel;
using ReInvented.ExcelInterop.Extensions;
using System.Windows.Input;
using ReInvented.Shared.Commands;
using System.IO;
using ReInvented.DataAccess.Services;
using ReInvented.DataAccess.Models;
using ReInvented.StaadPro.Interop.Services;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.Interop.Extensions;

using System.Collections.Generic;
using System.Linq;
using ReInvented.StaadPro.Interop.Entities;
using System.Collections.ObjectModel;
using ReInvented.DroopModifier.Extensions;
using ReInvented.DroopModifier.Interfaces;
using System.Runtime.InteropServices;
//using System.Windows;

namespace ReInvented.DroopModifier.ViewModels
{
    public class DroopModifierViewModel : ValidatablePropertyStore
    {
        #region Private Fields

        private static readonly double _tolerance = 0.001;

        #endregion

        #region Default Constructor

        public DroopModifierViewModel()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        public OpenStaadWrapper Wrapper { get => Get<OpenStaadWrapper>(); private set => Set(value); }

        public string StaadModelPath { get => Get<string>(); set { Set(value); RaisePropertyChanged(nameof(CanUpdateStaad)); } }

        public string ExcelFilePath
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaisePropertyChanged(nameof(CanUpdateStaad));
            }
        }

        public Node Origin { get => Get<Node>(); private set => Set(value); }

        public Feed Feed { get => Get<Feed>(); private set => Set(value); }

        public bool CanUpdateStaad => !string.IsNullOrWhiteSpace(ExcelFilePath) && !string.IsNullOrWhiteSpace(StaadModelPath) &&
            File.Exists(StaadModelPath) && File.Exists(ExcelFilePath);

        public HashSet<EntityGroup<Plate>> PlateEntityGroups { get => Get<HashSet<EntityGroup<Plate>>>(); private set => Set(value); }

        public ObservableCollection<Group> PlateGroups { get => Get<ObservableCollection<Group>>(); private set => Set(value); }

        public ObservableCollection<Group> SelectedGroups => new ObservableCollection<Group>(PlateGroups.Where(g => g.IsSelected));

        public int ItemsPerColumn { get; set; } = 10;

        public bool UpdateAtRadialBeamsLocationsAlso { get => Get<bool>(); set => Set(value); }

        public ObservableCollection<ObservableCollection<Group>> ColumnedPlateGroups
        { get => Get<ObservableCollection<ObservableCollection<Group>>>(); private set => Set(value); }


        #endregion

        #region Commands

        public ICommand BrowseStaadModelCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand BrowseExcelFileCommand { get => Get<ICommand>(); private set => Set(value); }

        public ICommand UpdateStaadModelCommand { get => Get<ICommand>(); private set => Set(value); }

        #endregion


        #region Private Helpers

        private void Initialize()
        {
            Origin = new Node(0.0, 0.0, 0.0);
            UpdateAtRadialBeamsLocationsAlso = false;
            BrowseStaadModelCommand = new RelayCommand(OnBrowseStaadModel, true);
            BrowseExcelFileCommand = new RelayCommand(OnBrowseExcelFile, true);
            UpdateStaadModelCommand = new RelayCommand(OnUpdateStaadModel, true);
        }

        private void UpdatePlateGroups()
        {
            if (Wrapper != null)
            {
                PlateEntityGroups = Wrapper.Geometry.GetEntityGroups<Plate>(1);
                PlateGroups = new ObservableCollection<Group>();
                PlateEntityGroups.ToList().ForEach(g => PlateGroups.Add(new Group() { IsSelected = false, Name = g.GroupName }));

                List<Group> plateGroupsList = PlateGroups.ToList();
                ColumnedPlateGroups = new ObservableCollection<ObservableCollection<Group>>();

                for (int i = 0; i < PlateGroups.Count; i += ItemsPerColumn)
                {
                    ColumnedPlateGroups.Add(new ObservableCollection<Group>(plateGroupsList.Skip(i).Take(ItemsPerColumn)));
                }

                RaiseMultiplePropertiesChanged(nameof(PlateGroups), nameof(ColumnedPlateGroups));
            }
        }

        private void UpdateSiteFeed()
        {
            Application excelApp = ExcelServices.Create();
            ApplicationExtensions.KillIfOpen(ExcelFilePath);

            Workbook workbook = excelApp.OpenWorkbook(ExcelFilePath);
            Worksheet worksheet = WorkbookExtensions.SheetByName(workbook, "Readings");

            Feed = new Feed
            {
                RadialSegmentsCount = (int)worksheet.Range["Nrb"].Value2,
                AlphaStart = worksheet.Range["AlphaFirst"].Value2,
                RadiusToZeroDroop = worksheet.Range["RDroopZero"].Value2
            };

            Range rngCircumferential = worksheet.Range["RngCircumferential"];
            Range rngRadial = worksheet.Range["RngRadial"];
            List<double> angles = new List<double>();
            List<double> radii = new List<double>();


            int sRowCirc = rngCircumferential.Row;
            int sColCirc = rngCircumferential.Column;
            int nRows = rngCircumferential.Rows.Count;

            for (int i = 0; i < nRows; i++)
            {
                dynamic value = worksheet.Cells[sRowCirc + i, sColCirc].Value2;

                if (value != null && Convert.ToString(value) != string.Empty)
                {
                    angles.Add(value);
                }
            }

            int sRowRadial = rngRadial.Row;
            int sColRadial = rngRadial.Column;
            int nCols = rngRadial.Columns.Count;
            int colSpan = 3;

            for (int i = 0; i < nCols; i += colSpan)
            {
                dynamic value = worksheet.Cells[sRowRadial, sColRadial + i].Value2;

                if (value != null && Convert.ToString(value) != string.Empty)
                {
                    radii.Add(value);
                }
            }

            Feed.Readings = new HashSet<IReading>();

            for (int iAngle = 0; iAngle < angles.Count; iAngle++)
            {
                for (int iRadius = 0; iRadius < radii.Count; iRadius++)
                {
                    dynamic droopDelta = worksheet.Cells[sRowCirc + iAngle, sColRadial + (iRadius * colSpan)].Value2;

                    Reading reading = new Reading(angles[iAngle], radii[iRadius], droopDelta);
                    _ = Feed.Readings.Add(reading);
                }
            }


            // Cleanup
            workbook.Close(false);
            excelApp.Quit();

            _ = Marshal.ReleaseComObject(worksheet);
            _ = Marshal.ReleaseComObject(workbook);
            _ = Marshal.ReleaseComObject(excelApp);

        }

        #endregion

        #region Command Handlers

        private void OnUpdateStaadModel()
        {
            if (PlateGroups.Count(pg => pg.IsSelected) <= 0)
            {
                _ = System.Windows.MessageBox.Show("No groups are selected. Please select atleast one group.", "Update Staad Model", System.Windows.MessageBoxButton.OK);
            }
            else
            {
                IEnumerable<EntityGroup<Plate>> selectedEntityGroups = PlateEntityGroups.Where(eg => SelectedGroups.Any(sg => sg.Name == eg.GroupName));
                HashSet<Node> uniqueNodes = selectedEntityGroups.SelectMany(eg => eg.Entities).SelectMany(p => p.GetNodes()).ToHashSet();

                HashSet<Node> modifiedNodes = new HashSet<Node>();

                if (UpdateAtRadialBeamsLocationsAlso)
                {
                    var unmodNodes = uniqueNodes;
                }
                else
                {
                    var modNodes = uniqueNodes.TakeWhile(n => !Feed.RadialBeamsLocations.Any(rbAngle => Math.Abs(rbAngle - Node.PlanAngleIn360DegreesOf(n, Origin)) <= _tolerance));
                }

                foreach (Node node in uniqueNodes)
                {
                    double nodeAngle = Node.PlanAngleIn360DegreesOf(node, Origin);

                    if (!Feed.RadialBeamsLocations.Any(rbAngle => Math.Abs(rbAngle - nodeAngle) <= _tolerance))
                    {
                        _ = modifiedNodes.Add(Feed.Readings.ModifyNode(Origin, node));
                    }
                }

                if (modifiedNodes.Count > 0)
                {
                    modifiedNodes.ToList().ForEach(n => Wrapper.Geometry.CreateNode(n.Id, n.X, n.Y, n.Z));
                }
            }
        }

        private void OnBrowseExcelFile()
        {
            ExcelFilePath = FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Excel files", "xls*"));

            if (!string.IsNullOrWhiteSpace(ExcelFilePath) && File.Exists(ExcelFilePath))
            {
                UpdateSiteFeed();
            }

        }

        private void OnBrowseStaadModel()
        {
            StaadModelPath = FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Staad model files", "std"));

            if (!string.IsNullOrWhiteSpace(StaadModelPath) && File.Exists(StaadModelPath))
            {
                Wrapper = OpenStaadWrapperProvider.Get(StaadModelPath);
                UpdatePlateGroups();
            }
        }

        #endregion

    }
}
