using Newtonsoft.Json;

using System;
using System.IO;
using System.Collections.Generic;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.DataAccess;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.Sections.Domain.Models;
using ReInvented.DataAccess.Services;
using ReInvented.DataAccess.NameProviders;
using System.Linq;
using System.Text;
using ReInvented.Shared.Services;

namespace SPro2023ConsoleApp
{

    class Program
    {
        public class Person
        {
            #region Parameterized Constructor

            public Person(string name, DateTime birthDay)
            {
                Name = name;
                BirthDay = birthDay;
            }

            #endregion

            #region ReadOnly Properties

            public string Name { get; private set; }

            public DateTime BirthDay { get; private set; }

            #endregion

            #region Static Functions

            public static List<Person> GetFamily()
            {
                return new List<Person>()
                {
                    new Person("Jamalaiah Masanam", new DateTime(1956, 06, 17)),
                    new Person("Jamalamma Masanam", new DateTime(1966, 03, 11)),
                    new Person("Srinivasa Rao Masanam", new DateTime(1985, 08, 17)),
                    new Person("Lakshmi Bhavani Masanam", new DateTime(1991, 02, 08)),
                    new Person("Vihan Masanam", new DateTime(2015, 06, 15)),
                    new Person("Likhita Masanam", new DateTime(2019, 03, 11))
                };
            }

            #endregion
        }

        private static void OnContentsChanged(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }


        static void Main(string[] args)
        {
            var watcher = new FileSystemWatcher(DirectoryPaths.Recent) { EnableRaisingEvents = true };

            var uid = UniqueIdentifierGenerator.GenerateUniqueId(watcher, 2);
            Console.WriteLine(uid);


            watcher.Changed -= OnContentsChanged;
            watcher.Changed += OnContentsChanged;

            //var other = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop";
            //var titlesName = "Titles.json";
            //var factorsName = "Factors.json";


            //var titles = File.ReadAllLines(Path.Combine(other, titlesName));
            //var factors = File.ReadAllLines(Path.Combine(other, factorsName));

            //var sId = 225;

            //var combinations = new List<string>();

            //for (int i = 0; i < titles.Length; i++)
            //{
            //    combinations.Add("{");
            //    combinations.Add($"\"Id\":{sId+i},");
            //    combinations.Add($"\"Title\":\"{titles[i]}\",");
            //    combinations.Add($"\"CombinationText\":\"{factors[i]}\"");
            //    combinations.Add("},");
            //}

            //File.WriteAllLines(Path.Combine(other, "Combinations.json"), combinations);




            //var directory = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo-Old\Delete\03. STAAD\01. Working";
            //var staadFileName = @"ASCE7-16.std";
            //var jsonFileName = @"ASCE7-16.json";

            //var lcd = LoadCombinationsDefinition.Parse(Path.Combine(directory, staadFileName), "ASCE7", 2016, "HRT", "Standard", new HashSet<Envelop>()
            //{
            //    new Envelop(EnvelopGroup.JointMass, 100, 100),
            //    new Envelop(EnvelopGroup.Strength, 101, 250),
            //    new Envelop(EnvelopGroup.Serviceability, 1001, 1170),
            //});

            //JsonDataSerializer<LoadCombinationsDefinition> serializer = new JsonDataSerializer<LoadCombinationsDefinition>();
            //File.WriteAllText(Path.Combine(directory, jsonFileName), serializer.Serialize(lcd, JsonSerializerSettingsProvider.Minified));



            //FileContentConversionService<SectionTables<RolledSectionOShape>> conversionService = new FileContentConversionService<SectionTables<RolledSectionOShape>>(ReInvented.DataAccess.Enums.ConversionMode.XmlToJson);
            //conversionService.Convert(@"D:\02. Due\00. Projects\03. Prodactivity\01. Templates\03. Final Excel Database Files\Pipes.srix", @"D:\02. Due\00. Projects\03. Prodactivity\01. Templates\03. Final Excel Database Files\CHS.json", JsonSerializerSettingsProvider.Minified);

            ////Stopwatch watch = new Stopwatch();
            ////watch.Start();
            ////watch.Stop();
            ////string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Time Stamp.json");
            ////File.WriteAllText(filePath, $"Total elapsed: {watch.Elapsed:hh\\:mm\\:ss}");

            ////DesignCoefficientsAndFactors designCoefficientsAndFactors = DesignCoefficientsAndFactors.ParseFromCsv(@"E:\SolutionsReInvented\BranchReorganization\MainProjects\SRi.XamlUIThickenerApp\ApplicationData\Assets\DataTables\CSV Backup\ASCE7-10RTable.csv");

            ////JsonDataSerializer<DesignCoefficientsAndFactors> serializer = new JsonDataSerializer<DesignCoefficientsAndFactors>();
            ////var serialized = serializer.Serialize(designCoefficientsAndFactors, JsonSerializerSettingsProvider.MinifiedSettings);
            ////File.WriteAllText(@"E:\SolutionsReInvented\BranchReorganization\MainProjects\SRi.XamlUIThickenerApp\ApplicationData\Assets\DataTables\ASCE7-10RTable_WIP_Test.json", serialized);




            ////ReportService.GenerateOptimizedSectionsSummary(@"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\", "Summary.docx");
            ////PublisherDirectoryMonitoringService.Initiate(@"\\10.67.12.30\product_engineering\Civil_Structural\ThickenerModelGenerator\");

            ////var primaryLoadCases = PrimaryLoadCasesProvider.GetFLDLoadCases();

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
