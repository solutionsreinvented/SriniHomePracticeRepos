using System;
using System.Collections.Generic;
using System.Windows;

using OpenSTAADUI;

using ReInvented.Shared.Models;
using ReInvented.StaadPro.Interop.Models;
//using ReInvented.StaadPro.Interop.Helpers;
using ReInvented.StaadPro.InteropCore.Models;
using ReInvented.StaadPro.InteropCore.Helpers;
using ReInvented.Shared.Helpers;

namespace ReInvented.StaadPro.InteropCore.Services
{
    public class OSWrapperCoreProvider
    {
        #region Main Functions

        public static OSWrapperCore Get(string fileFullPath = null)
        {
            try
            {
                IList<RotItem<OpenSTAAD>> runningInstances = RotHelpers.GetAllRunningInstances<OpenSTAAD>();

                OpenStaadWrapper osWrapper = runningInstances.Count <= 0
                    ? OpenStaadWrapperHelpersCore.GetWhenNoRunningInstancesExists(fileFullPath)
                    : OpenStaadWrapperHelpersCore.GetWhenRunningInstancesExists(fileFullPath);

                return new OSWrapperCore(osWrapper);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Could not acquire an OpenStaad object. Possible reason could be that a running instance of Staad.Pro is not found! " +
                    $"In order to create the OpenStaad object, please ensure that the Staad.Pro application is running. For more information refer to the following details. {Environment.NewLine}{ex.Message}",
                    "Get OpenStaad Objects", MessageBoxButton.OK);
            }

            return new OSWrapperCore(null);
        }

        #endregion
    }
}
