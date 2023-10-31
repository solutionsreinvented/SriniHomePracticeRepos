﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.StaadPro.Interactivity.Services;

using SPro2023ConsoleApp.Services;

namespace SPro2023ConsoleApp
{

    public class UnitsData
    {
        public string Length { get; set; }

        public string Display { get; set; }

        public string Angle { get; set; }

        public string Weight { get; set; }
    }

    public class PlateMtoRow
    {
        public double Thickness { get; set; }

        public int ObjectsCount { get; set; }

        public double TotalPlanArea { get; set; }

        public double TotalWeight { get; set; }

    }

    public class BeamMtoRow
    {
        public string PropertyName { get; set; }

        public int ObjectsCount { get; set; }

        public double TotalLength { get; set; }

        public double TotalWeight { get; set; }

    }

    public class ImmediateTakeOff
    {
        public static void GeneratePlatesMtoTableInStaadModel(StaadModelWrapper wrapper)
        {
            object staadFile = string.Empty;

            wrapper.StaadInstance.GetSTAADFile(ref staadFile, "TRUE");

            if (!string.IsNullOrWhiteSpace((string)staadFile))
            {
                //Get member data
                OSPropertyUI property = wrapper.StaadInstance.Property;
                OSGeometryUI geometry = wrapper.StaadInstance.Geometry;

                int nPlates = geometry.GetPlateCount();
                object plates = new int[nPlates];
                geometry.GetPlateList(ref plates);

                int nColumns = 4;
                int nRows = property.GetThicknessPropertyCount();

                Dictionary<int, PlateMtoRow> propertiesTable = new Dictionary<int, PlateMtoRow>(nRows);

                (int ReportId, int TableId) tableData = CreateTable(wrapper.StaadInstance, nRows, nColumns);

                string[] columnHeaders = GetPlatesMtoTableColumnHeadersContent(wrapper.StaadInstance, nRows, nColumns, tableData.ReportId, tableData.TableId);
                string[] columnUnits = GetTableColumnUnitsContent(wrapper.StaadInstance, nRows, nColumns, tableData.ReportId, tableData.TableId);

                GenerateTableHeaders(wrapper.StaadInstance, tableData.ReportId, tableData.TableId, nColumns, columnHeaders, columnUnits);

                int propertyId;

                int[] PlatesArray = (int[])plates;

                for (int iPlate = 0; iPlate < nPlates; iPlate++)
                {
                    int PlateId = PlatesArray[iPlate];

                    propertyId = property.GetPlateThicknessPropertyRefNo(PlateId);

                    PlateMtoRow row;

                    if (propertiesTable.Keys.Contains(propertyId))
                    {
                        row = propertiesTable[propertyId];
                    }
                    else
                    {
                        row = new PlateMtoRow();
                        propertiesTable.Add(propertyId, row);
                    }

                    row.ObjectsCount += 1;
                    //row.TotalLength += Math.Round(property.Get.GetBeamLength(PlateId), 3);
                }

                for (int iRow = 0; iRow < nRows; iRow++)
                {
                    propertyId = propertiesTable.Keys.ToList()[iRow];
                    PlateMtoRow row = propertiesTable[propertyId];

                    object plateThickness = new double[4];

                    property.GetPlateThickness(propertyId, ref plateThickness);

                    int propertyAssignedCount = property.GetThicknessPropertyAssignedPlateCount(propertyId);

                    object propertyAssignedList = new int[propertyAssignedCount];

                    property.GetThicknessPropertyAssignedPlateList(propertyId, ref propertyAssignedList);

                    row.Thickness = Math.Round(((double[])plateThickness).Average(), 5);
                    row.TotalPlanArea = PlateSurfaceAreaService.Calculate(wrapper, ((int[])propertyAssignedList).ToHashSet());
                   

                    /// Notes:
                    ///     1. First value in the propertyValues gives sectional area for most of the section types.
                    ///        Introduce additional conditions or use a separate class to retrieve the sectional area for sections
                    ///        which does not contain the sectional area in the propertyValues array.
                    ///     2. To calculate the multiply the total length with sectional area and then with density of the material.
                    ///        In the below line of code a value 7.85 (t/m³) is used directly for testing purposes.
                    ///        Actual weight has to be calculated using the density that is specific to the material selected.

                    row.TotalWeight = Math.Round(row.TotalPlanArea * row.Thickness * 7.85, 3);
                }

                FillPlatesMtoTable(wrapper.StaadInstance, propertiesTable, tableData.ReportId, tableData.TableId);
            }
        }


        public static void GenerateSectionsMtoTableInStaadModel(OpenSTAAD instance)
        {
            object staadFile = string.Empty;

            instance.GetSTAADFile(ref staadFile, "TRUE");

            if (!string.IsNullOrWhiteSpace((string)staadFile))
            {
                //Get member data
                OSPropertyUI property = instance.Property;
                OSGeometryUI geometry = instance.Geometry;

                int nBeams = geometry.GetMemberCount();
                object beams = new int[nBeams];
                geometry.GetBeamList(ref beams);

                int nColumns = 4;
                int nRows = property.GetSectionPropertyCount();

                Dictionary<int, BeamMtoRow> propertiesTable = new Dictionary<int, BeamMtoRow>(nRows);

                (int ReportId, int TableId) tableData = CreateTable(instance, nRows, nColumns);

                string[] columnHeaders = GetSectionsMtoTableColumnHeadersContent(instance, nRows, nColumns, tableData.ReportId, tableData.TableId);
                string[] columnUnits = GetTableColumnUnitsContent(instance, nRows, nColumns, tableData.ReportId, tableData.TableId);

                GenerateTableHeaders(instance, tableData.ReportId, tableData.TableId, nColumns, columnHeaders, columnUnits);

                int propertyId;

                int[] beamsArray = (int[])beams;

                for (int iBeam = 0; iBeam < nBeams; iBeam++)
                {
                    int beamId = beamsArray[iBeam];

                    propertyId = property.GetBeamSectionPropertyRefNo(beamId);

                    BeamMtoRow row;

                    if (propertiesTable.Keys.Contains(propertyId))
                    {
                        row = propertiesTable[propertyId];
                    }
                    else
                    {
                        row = new BeamMtoRow();
                        propertiesTable.Add(propertyId, row);
                    }

                    row.ObjectsCount += 1;
                    row.TotalLength += Math.Round(geometry.GetBeamLength(beamId), 3);
                }

                for (int iRow = 0; iRow < nRows; iRow++)
                {
                    propertyId = propertiesTable.Keys.ToList()[iRow];
                    BeamMtoRow row = propertiesTable[propertyId];

                    object propertyName = string.Empty;

                    property.GetSectionPropertyName(propertyId, ref propertyName);

                    row.PropertyName = (string)propertyName;


                    int count = property.GetCountofSectionPropertyValuesEx();
                    object propertyValues = new double[count];
                    object propertyType = 0;

                    property.GetSectionPropertyValuesEx(propertyId, ref propertyType, ref propertyValues);

                    /// Notes:
                    ///     1. First value in the propertyValues gives sectional area for most of the section types.
                    ///        Introduce additional conditions or use a separate class to retrieve the sectional area for sections
                    ///        which does not contain the sectional area in the propertyValues array.
                    ///     2. To calculate the multiply the total length with sectional area and then with density of the material.
                    ///        In the below line of code a value 7.85 (t/m3) is used directly for testing purposes.
                    ///        Actual weight has to be calculated using the density that is specific to the material selected.

                    row.TotalWeight = Math.Round(((double[])propertyValues)[0] * row.TotalLength * 7.85, 3);
                }

                FillSectionsMtoTable(instance, propertiesTable, tableData.ReportId, tableData.TableId);
            }
        }

        private static string[] GetSectionsMtoTableColumnHeadersContent(OpenSTAAD instance, int nRows, int nColumns, int reportId, int tableId)
        {
            string[] columnHeaders = new string[nColumns];

            columnHeaders[0] = "Profile";
            columnHeaders[1] = "Number of Objects";
            columnHeaders[2] = "Total Length";
            columnHeaders[3] = "Total Weight";

            return columnHeaders;
        }

        private static string[] GetPlatesMtoTableColumnHeadersContent(OpenSTAAD instance, int nRows, int nColumns, int reportId, int tableId)
        {
            string[] columnHeaders = new string[nColumns];

            columnHeaders[0] = "Plate";
            columnHeaders[1] = "Number of Objects";
            columnHeaders[2] = "Total Plan Area";
            columnHeaders[3] = "Total Weight";

            return columnHeaders;
        }

        private static string[] GetTableColumnUnitsContent(OpenSTAAD instance, int nRows, int nColumns, int reportId, int tableId)
        {
            string[] columnUnits = new string[nColumns];

            UnitsData units = new UnitsData();

            switch (instance.GetBaseUnit())
            {
                case 1:
                    units.Length = "ft";
                    units.Display = "in";
                    units.Angle = "rad";
                    units.Weight = "MT";
                    break;
                case 2:
                    units.Length = "m";
                    units.Display = "mm";
                    units.Angle = "rad";
                    units.Weight = "MT";
                    break;
                default:
                    _ = MessageBox.Show($"Base unit \"{instance.GetBaseUnit()}\" is undefined");
                    break;
            }

            columnUnits[0] = string.Empty;
            columnUnits[1] = string.Empty;
            columnUnits[2] = $"({units.Length})";
            columnUnits[3] = $"({units.Weight})";

            return columnUnits;
        }

        private static (int ReportId, int TableId) CreateTable(OpenSTAAD instance, int nRows, int nColumns)
        {
            OSTableUI table = instance.Table;

            int reportId = table.CreateReport("Take Off");

            //Create sheets
            //Table name, number of rows and number of columns
            int tableId = table.AddTable(reportId, "Take Off", nRows, nColumns);

            return (reportId, tableId);
        }

        private static void GenerateTableHeaders(OpenSTAAD instance, int reportId, int tableId, int nColumns, string[] columnHeaders, string[] columnUnits)
        {
            OSTableUI table = instance.Table;

            for (int iCol = 0; iCol < nColumns; iCol++)
            {
                table.SetColumnHeader(reportId, tableId, iCol + 1, columnHeaders[iCol]);
                table.SetColumnUnitString(reportId, tableId, iCol + 1, columnUnits[iCol]);
            }
        }

        private static void FillSectionsMtoTable(OpenSTAAD instance, Dictionary<int, BeamMtoRow> propertiesTable, int reportId, int tableId)
        {
            OSTableUI table = instance.Table;

            for (int iRow = 0; iRow < propertiesTable.Values.Count(); iRow++)
            {
                BeamMtoRow row = propertiesTable.Values.ToList()[iRow];

                table.SetCellValue(reportId, tableId, iRow + 1, 1, row.PropertyName);
                table.SetCellValue(reportId, tableId, iRow + 1, 2, row.ObjectsCount);
                table.SetCellValue(reportId, tableId, iRow + 1, 3, row.TotalLength);
                table.SetCellValue(reportId, tableId, iRow + 1, 4, row.TotalWeight);
            }

        }
        private static void FillPlatesMtoTable(OpenSTAAD instance, Dictionary<int, PlateMtoRow> propertiesTable, int reportId, int tableId)
        {
            OSTableUI table = instance.Table;

            for (int iRow = 0; iRow < propertiesTable.Values.Count(); iRow++)
            {
                PlateMtoRow row = propertiesTable.Values.ToList()[iRow];

                table.SetCellValue(reportId, tableId, iRow + 1, 1, $"{row.Thickness * 1000}mm THK.");
                table.SetCellValue(reportId, tableId, iRow + 1, 2, row.ObjectsCount);
                table.SetCellValue(reportId, tableId, iRow + 1, 3, row.TotalPlanArea);
                table.SetCellValue(reportId, tableId, iRow + 1, 4, row.TotalWeight);
            }

        }
    }
}
