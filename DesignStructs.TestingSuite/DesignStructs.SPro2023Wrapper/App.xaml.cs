using System;
using System.IO;
using System.Threading;
using System.Windows;

using Microsoft.Win32;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.StaadPro.Interactivity.Services;

namespace ReInvented.SPro2023Wrapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StaadModel model = new StaadModel();

            string filePath = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD\D11.3H03.00S09.00OC1.050SC1.128IMP0.439CON0.1665MOT0054.std";

            GenerateGeometryInStaadModel(filePath, model);
        }

        private static void EnsureCreate(OpenSTAAD instance, string fullFilePath)
        {

        }

        public static void GenerateGeometryInStaadModel(string filePath, StaadModel model)
        {
            OpenSTAAD instance = model.StaadWrapper.StaadInstance;

            //SaveFileDialog saveFileDialog = new SaveFileDialog()
            //{
            //    FileName = filePath,
            //    Filter = "Staad Models (*.std) | *.std",
            //    InitialDirectory = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD"
            //};

            //if (saveFileDialog.ShowDialog() == true)
            //{
            //    wrapper.StaadInstance.NewSTAADFile(saveFileDialog.FileName, LengthInputUnit.Meter, ForceInputUnit.KiloNewton);


            //    /// Note: Below functions shall be used only after the beams are generated.
            //    /// TODO: Think of the possibility to move the below operations to the ThickenerProcessor class
            //    ///SectionPropertyServices.CreateAndAssignSections(wrapper, model.Beams);
            //    ///SectionPropertyServices.CreateAndAssignOffsetSpecifications(wrapper, model.Beams);
            //}

            if (!StaadModelServices.IsModelOpen(instance, filePath))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    FileName = Path.GetFileName(filePath),
                    Filter = "Staad Models (*.std) | *.std",
                    InitialDirectory = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    instance.OpenSTAADFile(filePath);
                }
            }
            if (StaadModelServices.IsModelAccessible(instance, filePath, 5 * 60))
            {
                GenerateGeometricalEntities(instance.Geometry);
                instance.SaveModel(1);
                instance.CloseSTAADFile();
                ///instance.Quit();
            }

        }

        private static void GenerateGeometricalEntities(OSGeometryUI geometry)
        {
            for (int i = 0; i < 1000; i++)
            {
                geometry.CreateNode(i + 1, i * 2, i * 0.5, i * 1.5);
            }
        }
    }

}
