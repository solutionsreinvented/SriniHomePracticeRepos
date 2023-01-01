using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

using Abstracture.Infrastructure.Services;
using Microsoft.Win32;

namespace Abstracture.Infrastructure
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static IEnumerable<RegistryKey> GetSubKeysFor(RegistryKey key, string subKeyName)
        {
            return key.GetSubKeyNames().Select(n => key.OpenSubKey(n));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var root = Registry.ClassesRoot;
            var staadKeyName = root.GetSubKeyNames().FirstOrDefault(k => k.ToUpperInvariant() == "STAADPRO");
            var staadKey = root.OpenSubKey(staadKeyName);

            var subKeys = staadKey.GetSubKeyNames().Select(n => staadKey.OpenSubKey(n));
            foreach (var item in subKeys)
            {
                if (item.GetSubKeyNames().Count() <= 0)
                {
                    var values = item.ValueCount;
                }
                else
                {

                }
            }


            HandsOn.RootFunctions();


            ///base.OnStartup(e);
        }
    }
}
