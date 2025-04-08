using System;
using System.Windows;

using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.Interop.Services;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Services
{
    public class OSWrapperCoreProvider
    {
        #region Main Functions

        public static OSWrapperCore Get(string fileFullPath = null)
        {
            try
            {
                OpenStaadWrapper osWrapper = OpenStaadWrapperProvider.Get(fileFullPath);
                return new OSWrapperCore(osWrapper);
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Could not acquire an OpenStaad object. Possible reason could be that a running instance of Staad.Pro is not found!" +
                    "In order to create the OpenStaad object, please ensure that the Staad.Pro application is running.", "Get OpenStaad Objects", MessageBoxButton.OK);
            }

            return new OSWrapperCore(null);
        }

        #endregion
    }
}
