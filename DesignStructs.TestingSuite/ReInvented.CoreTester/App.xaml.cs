using System.Collections.Generic;
using System.Windows;

using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.InteropCore.Interfaces;
using ReInvented.StaadPro.InteropCore.Models;
using ReInvented.StaadPro.InteropCore.Services;

namespace ReInvented.CoreTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string staadModelPath = ""; ///FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Staad model files", "std"));
            OSWrapperCore wrapperCore = OSWrapperCoreProvider.Get(staadModelPath);
            IOSRootCore osRoot = wrapperCore.OSRootCore;

            List<int> beams = new() { 15440, 15441 };
            List<int> loadCases = new() { 61, 62 };

            HashSet<MemberForces> memForces = osRoot.RetrieveMemberForces(beams, loadCases);
        }
    }
}
