using System.Collections.Generic;
using System.Windows;

using ReInvented.DroopModifier.Extensions;
using ReInvented.DroopModifier.Interfaces;
using ReInvented.DroopModifier.Models;
using ReInvented.DroopModifier.ViewModels;
using ReInvented.DroopModifier.Views;
using ReInvented.StaadPro.Interop.Entities;

namespace ReInvented.DroopModifier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //List<IReading> readings = new List<IReading>();
            //int nSeg = 20;
            //int nRad = 10;
            //double rStart = 1.450;
            //double rInc = 3.400;
            //double sAngle = 9.0;
            //double angleInc = 360.0 / nSeg;

            //for (int s = 0; s < nSeg; s++)
            //{
            //    for (int r = 0; r < nRad; r++)
            //    {
            //        readings.Add(new Reading(sAngle + (s * angleInc), rStart + (r * rInc), 0.00));
            //    }
            //}
            //Node origin = new Node(0.0, 0.0, 0.0);
            //Node target = new Node(-11.5, 3.6, -2.89);

            //readings.ModifyNode(origin, target);


            base.OnStartup(e);
            MainWindow = new DroopModifierView() { DataContext = new DroopModifierViewModel() };
            MainWindow.Show();
        }
    }
}
