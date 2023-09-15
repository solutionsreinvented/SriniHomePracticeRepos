using System;
using System.Windows;

using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Sections.Domain.Models;
using System.Collections.ObjectModel;
using System.Linq;
using ReInvented.ExcelInteropDesign.Services;
using ReInvented.Sections.Domain.Services;
using ReInvented.ExcelInteropDesign.ViewModels;
using ReInvented.StaadPro.Interactivity.Models;
using System.Collections.Generic;
using ReInvented.ExcelInteropDesign.Models;

namespace ReInvented.ExcelInteropDesign
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ///SectionDatabaseFileConversionService.ConvertAllFrom(@"E:\SolutionsReInvented\BranchReorganization\MainProjects\SRi.XamlUIThickenerApp\ApplicationData\Assets\Sections\Xml");

            ///RolledSectionHShape section = PreviousSectionSelection();

            ///ExcelInterop();


            StaadModel model = new StaadModel();

            List<int> beams = new List<int>();
            beams.AddRange(Enumerable.Range(5761, 8));

            List<int> loadcases = new List<int>();
            loadcases.AddRange(Enumerable.Range(101, 10));

            HashSet<MemberForces> allforces = StaadOutputServices.RetrieveForces(model.StaadWrapper, beams, loadcases);
            List<MemberForces> summarized = StaadOutputServices.SummarizeForces(allforces);
            

            MainWindow = new SectionChoicesWindow() { DataContext = new SectionsPreferenceViewModel() };

            MainWindow.Show();
        }

        private void ExcelInterop(RolledSectionHShape section)
        {

        }

        private static RolledSectionHShape PreviousSectionSelection()
        {
            ObservableCollection<Database> databases = SectionsRepository.Instance.GetSectionsLibrary().Databases;

            Database database = databases.FirstOrDefault();
            RolledSectionHShape section = database.SectionShapes.FirstOrDefault().Classifications.FirstOrDefault(c => c.Category == "MB Sections").Sections.FirstOrDefault(s => s.Designation.Contains("ISMB400")) as RolledSectionHShape;
            return section;
        }
    }
}
