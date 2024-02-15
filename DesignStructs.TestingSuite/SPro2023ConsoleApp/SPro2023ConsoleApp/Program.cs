using Newtonsoft.Json;

using OpenSTAADUI;

using ReInvented.Domain.Tass.Common.Services;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SPro2023ConsoleApp
{

    public class TestClass
    {
        public double Property1 { get; set; }

        [ClassRecog]
        public double Property2 { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ClassRecogAttribute : Attribute
    {
        // You can add additional properties or methods to the attribute if needed
    }

    class Program
    {
        // Method to get instances of a given type
        static IEnumerable<object> GetInstances(Type type)
        {
            // Get all loaded assemblies
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Iterate over each assembly and get instances of the given type
            foreach (Assembly assembly in assemblies)
            {
                // Get all types in the assembly
                IEnumerable<Type> allTypes = assembly.GetTypes();

                // Filter types to find those assignable to the given type
                var compatibleTypes = allTypes.Where(t => type.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

                // Iterate over compatible types and create instances
                foreach (Type compatibleType in compatibleTypes)
                {
                    // Create an instance of the type
                    object instance = Activator.CreateInstance(compatibleType);
                    yield return instance;
                }
            }
        }

        static void Main(string[] args)
        {
            // Get all loaded assemblies
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Iterate over each assembly
            foreach (Assembly assembly in assemblies)
            {
                // Get all types in the assembly
                IEnumerable<Type> allTypes = assembly.GetTypes().Where(t => t.GetProperties().Any(prop => Attribute.IsDefined(prop, typeof(ClassRecogAttribute))));

                // Iterate over each type
                foreach (Type type in allTypes)
                {
                    // Check if any instance of the type exists
                    var instances = GetInstances(type);

                    // If instances exist, do something with them
                    if (instances.Any())
                    {
                        Console.WriteLine($"Instances of type '{type.FullName}' with properties decorated with ClassRecog attribute:");

                        // Print details of each instance
                        foreach (var instance in instances)
                        {
                            Console.WriteLine($"Instance: {instance}");
                            // Access properties of instance here if needed
                        }
                    }

                }
            }


            string filePath = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Desktop\STAAD\Multi Instances\D35.0H2.50S09.00OC1.113SC1.219IMP0.240CON0.0300MOT1250_2.std"; // Replace with the actual file path

            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Open the file using its default application
                Process.Start(filePath);
            }


            StaadModel model = new StaadModel();
            OpenSTAAD OpenStaad = model.OpenStaadWrapper.StaadInstance;

            OSDesignUI design = OpenStaad.Design;
            OSMemberSteelDgnParams osMemberSteelDgnParams = new OSMemberSteelDgnParams();
            design.GetMemberDesignParameters(1, 25354, osMemberSteelDgnParams);

            IEnumerable<IRolledSection> sections = SectionsRepository.Instance.GetSectionsLibrary().AllSections;
            int count = sections.Count();
            sections.ToList().ForEach(s => Console.WriteLine($"{s.Designation}"));
            //LoadListCreationTest();




            Console.ReadLine();

            ///await Previous();
        }

        private static void LoadListCreationTest()
        {
            StaadModel model = new StaadModel();
            OSLoadUI OSLoad = model.OpenStaadWrapper.Load as OSLoadUI;
            OpenSTAAD OpenStaad = model.OpenStaadWrapper.StaadInstance;

            OSLoad.CreateLoadList(LoadListType.EnvelopList, 1);

            IEnumerable<Node> nodes = (model.OpenStaadWrapper.Geometry as OSGeometryUI).GetUniqueNodesFromPlateGroup(GroupNames.CapPlateOuter);
            Node origin = new Node(0.0, nodes.FirstOrDefault().Y, 0.0);
            double leastRadius = nodes.Select(n => n.Radius(origin)).Min();
            _ = nodes.Select(n => n.Radius(origin) <= leastRadius);
        }

        private static NodesCollection ReadNodesFromJson()
        {
            string fileContents = File.ReadAllText("NodesCollection.json");
            NodesCollection deserialized = JsonConvert.DeserializeObject<NodesCollection>(fileContents);

            return deserialized;
        }
    }
}
