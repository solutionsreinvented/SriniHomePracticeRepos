using System;
using System.Reflection;

namespace DumpClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            DumpAssembly(@"F:\06. ReInvented\MainProjects\SRi.XamlUIThickenerApp\ReInvented.Licensing.Core\bin\Debug\ReInvented.Licensing.Core.dll");
        }

        static void DumpAssembly(string path)
        {
            try
            {
                var assembly = Assembly.LoadFrom(path);
                Console.WriteLine($"--- {assembly.GetName().Name} ---");
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsPublic)
                    {
                        if (type.Name == "ApplicationModule")
                        {
                            Console.WriteLine("Enum: " + type.FullName);
                            foreach (var name in Enum.GetNames(type))
                            {
                                Console.WriteLine($"  - {name}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {path}: {ex.Message}");
            }
        }
    }
}
