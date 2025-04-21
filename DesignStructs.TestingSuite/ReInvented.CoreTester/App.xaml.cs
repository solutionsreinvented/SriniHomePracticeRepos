using System.Collections.Generic;
using System.Windows;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.Interop.Core.Models;
using ReInvented.StaadPro.Interop.Core.Services;
using ReInvented.StaadPro.Interop.Core.Extensions;

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

            KeepLoopingHelper(coreWrapper);
        }

        private void KeepLoopingHelper(OpenStaadCoreWrapper coreWrapper)
        {

            OSCoreRoot root = coreWrapper.Root;
            OSCoreGeometry geometry = coreWrapper.Geometry;
            OSCoreOutput output = coreWrapper.Output;
            OSCoreLoad load = coreWrapper.Load;
            OSCoreProperty property = coreWrapper.Property;
            OSCoreSupport support = coreWrapper.Support;


            HashSet<Plate> allPlates = geometry.GetAllEntities<Plate>(10);
            //IEnumerable<string> grpNames = geometry.GetAllGroupNames();
            //IEnumerable<int> allBeams = geometry.GetAllBeamsList();

            IGenericSection section = property.GetSectionFrom(11);
            TCISection actSection = section as TCISection;
            IEnumerable<ILoadCase> pLoadCases = load.GetAllLoadCasesOfType(LoadCaseType.PrimaryLoad);
            //PlateCenterResults result = output.GetPlateCenterResults(pLoadCases.First(), allPlates.First());
            HashSet<Node> unknown = support.GetAllSupportNodes(geometry, 10);

            //Throwing Methods
            //HashSet<MemberForces> memForces = root.RetrieveMemberForces(beams, loadCases);

            KeepLoopingHelper(coreWrapper);
        }

    }
}
