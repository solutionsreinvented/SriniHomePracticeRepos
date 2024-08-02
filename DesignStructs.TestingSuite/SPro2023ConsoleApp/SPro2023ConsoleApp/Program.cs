using Newtonsoft.Json;

using SPro2023ConsoleApp.Services;

using System;
using System.IO;
using ReInvented.Domain.Tass.Services;
using SPro2023ConsoleApp.Models;
using ReInvented.DataAccess;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.EarthquakeLoading.Models;
using System.Timers;
using System.Windows;
using System.Diagnostics;
using System.Reflection;
using ReInvented.Sections.Domain.Models;

namespace SPro2023ConsoleApp
{

    class Program
    {
        static void Main(string[] args)
        {
            FileContentConversionService<SectionTables<RolledSectionOShape>> conversionService = new FileContentConversionService<SectionTables<RolledSectionOShape>>(ReInvented.DataAccess.Enums.ConversionMode.XmlToJson);
            conversionService.Convert(@"D:\02. Due\00. Projects\03. Prodactivity\01. Templates\03. Final Excel Database Files\Pipes.srix", @"D:\02. Due\00. Projects\03. Prodactivity\01. Templates\03. Final Excel Database Files\CHS.json", JsonSerializerSettingsProvider.Minified);

            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //watch.Stop();
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Time Stamp.json");
            //File.WriteAllText(filePath, $"Total elapsed: {watch.Elapsed:hh\\:mm\\:ss}");

            //DesignCoefficientsAndFactors designCoefficientsAndFactors = DesignCoefficientsAndFactors.ParseFromCsv(@"E:\SolutionsReInvented\BranchReorganization\MainProjects\SRi.XamlUIThickenerApp\ApplicationData\Assets\DataTables\CSV Backup\ASCE7-10RTable.csv");

            //JsonDataSerializer<DesignCoefficientsAndFactors> serializer = new JsonDataSerializer<DesignCoefficientsAndFactors>();
            //var serialized = serializer.Serialize(designCoefficientsAndFactors, JsonSerializerSettingsProvider.MinifiedSettings);
            //File.WriteAllText(@"E:\SolutionsReInvented\BranchReorganization\MainProjects\SRi.XamlUIThickenerApp\ApplicationData\Assets\DataTables\ASCE7-10RTable_WIP_Test.json", serialized);




            //ReportService.GenerateOptimizedSectionsSummary(@"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\", "Summary.docx");
            //PublisherDirectoryMonitoringService.Initiate(@"\\10.67.12.30\product_engineering\Civil_Structural\ThickenerModelGenerator\");

            //var primaryLoadCases = PrimaryLoadCasesProvider.GetFLDLoadCases();

            Console.ReadLine();

            ///await Previous();
        }

        private static NodesCollection ReadNodesFromJson()
        {
            string fileContents = File.ReadAllText("NodesCollection.json");
            NodesCollection deserialized = JsonConvert.DeserializeObject<NodesCollection>(fileContents);

            return deserialized;
        }
    }
}
