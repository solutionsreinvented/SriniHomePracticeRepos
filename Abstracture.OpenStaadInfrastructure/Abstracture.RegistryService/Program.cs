using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracture.OpenStaadInfrastructure.Extensions;
using Abstracture.Infrastructure.Services;
using OpenSTAADUI;
using System.Threading;

namespace Abstracture.RegistryService
{
    class Program
    {


        private static IEnumerable<RegistryKey> RetrieveKeysRecursivelyFrom(RegistryKey registryKey, List<RegistryKey> allKeys)
        {
            var subKeys = registryKey.GetSubKeyNames().Select(s => registryKey.OpenSubKey(s));

            foreach (var key in subKeys)
            {
                allKeys.Add(key);
                RetrieveKeysRecursivelyFrom(key, allKeys);
            }

            return allKeys;
        }


        static void Main(string[] args)
        {
            IDictionary<string, object> allKeyValuePairs = new Dictionary<string, object>();
            List<RegistryKey> allKeys = new List<RegistryKey>();

            var classesRoot = Registry.ClassesRoot;
            var staadKeyName = classesRoot.GetSubKeyNames().FirstOrDefault(s => s.ToUpperInvariant() == "STAADPRO");
            var staadKey = classesRoot.OpenSubKey(staadKeyName);

            ///var staadSubKeys = RetrieveKeysRecursivelyFrom(staadKey, allKeys);

            var staadSubKeys = staadKey.AddKeysRecursivelyTo(allKeys);

            var staadExecutable = allKeys.SkipWhile(k => k.GetValueNames().Count() <= 0)
                                         .KeyContainingStringInItsValue("Staadpro.exe")
                                         .FullValueContaining("Staadpro.exe");


            var staadExecutable2 = staadKey.AddKeysRecursivelyTo(allKeys)
                                           .SkipWhile(k => k.GetValueNames().Count() <= 0)
                                           .KeyContainingStringInItsValuePrev("Staadpro.exe")
                                           .FullValueContaining("Staadpro.exe");

            var process = Process.Start(staadExecutable);
            
            Thread.Sleep(1000);

            OpenSTAAD instance = OpenStaadService.Instance();
            instance.SetSilentMode(true);

            Console.ReadLine();
        }
    }
}
