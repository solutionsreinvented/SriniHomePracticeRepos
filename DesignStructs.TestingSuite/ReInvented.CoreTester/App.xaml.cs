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
            OpenStaadCoreWrapper coreWrapper = OSWrapperCoreProvider.Get(staadModelPath);
            OSCoreRoot root = coreWrapper.Root;
            OSCoreGeometry geometry = coreWrapper.Geometry;
            
            HashSet<Plate> allPlates = geometry.GetAllEntities<Plate>(10);
            //geometry.DeleteExistingGeometry(5);
            IEnumerable<string> dontKnow = geometry.GetAllGroupNames();

            List<int> beams = new() { 15440, 15441 };
            List<int> loadCases = new() { 61, 62 };

            HashSet<MemberForces> memForces = root.RetrieveMemberForces(beams, loadCases);
        }
    }
}
