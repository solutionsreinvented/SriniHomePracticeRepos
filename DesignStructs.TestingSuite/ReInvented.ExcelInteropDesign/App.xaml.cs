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

            MainWindow = new SectionChoicesWindow() { DataContext = new SectionChoicesViewModel() };

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
