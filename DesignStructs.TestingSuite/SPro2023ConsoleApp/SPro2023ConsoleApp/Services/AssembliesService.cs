using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SPro2023ConsoleApp.Services
{
    public class ClassRecogAttribute : Attribute
    {

    }

    public class AssembliesService
    {
        public static void UnknownOperation()
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
        }


        // Method to get instances of a given type
        public static IEnumerable<object> GetInstances(Type type)
        {
            // Get all loaded assemblies
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Iterate over each assembly and get instances of the given type
            foreach (Assembly assembly in assemblies)
            {
                // Get all types in the assembly
                IEnumerable<Type> allTypes = assembly.GetTypes();

                // Filter types to find those assignable to the given type
                IEnumerable<Type> compatibleTypes = allTypes.Where(t => type.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

                // Iterate over compatible types and create instances
                foreach (Type compatibleType in compatibleTypes)
                {
                    // Create an instance of the type
                    object instance = Activator.CreateInstance(compatibleType);
                    yield return instance;
                }
            }
        }
    }
}
