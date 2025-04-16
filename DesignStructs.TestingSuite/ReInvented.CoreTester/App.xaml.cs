using System.Collections.Generic;
using System.Windows;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.InteropCore.Models;
using ReInvented.StaadPro.InteropCore.Services;
using ReInvented.StaadPro.InteropCore.Extensions;
using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using System.Threading.Tasks;

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
            string staadModelPath = FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Staad model files", "std"));
            OSWrapperCore wrapper = OSWrapperCoreProvider.Get(staadModelPath);
            OpenStaadCore openStaad = wrapper.OpenStaad;
            OSGeometryCore geometry = wrapper.Geometry;

            var allPlates = geometry.GetAllPlatesList();

            IEnumerable<string> dontKnow = geometry.GetAllGroupNames();

            List<int> beams = new() { 15440, 15441 };
            List<int> loadCases = new() { 61, 62 };

            HashSet<MemberForces> memForces = openStaad.RetrieveMemberForces(beams, loadCases);
        }
    }
}
