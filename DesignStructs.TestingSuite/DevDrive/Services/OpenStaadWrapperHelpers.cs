using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DevDrive.Enums;
using DevDrive.Models;

using OpenSTAADUI;

using ReInvented.DataAccess.Services;
using ReInvented.Shared.Extensions;
using ReInvented.Shared.Helpers;
using ReInvented.Shared.Interfaces;
using ReInvented.Shared.Models;
using ReInvented.Shared.Services;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Helpers;
using ReInvented.StaadPro.Interactivity.Models;

namespace DevDrive.Services
{
    public class OpenStaadWrapperProvider
    {

        #region Main Functions

        public static OpenStaadWrapper Get(string fileFullPath = null)
        {
            try
            {
                IList<RotItem<OpenSTAAD>> runningInstances = RotHelpers.GetAllRunningInstances<OpenSTAAD>();

                return runningInstances.Count <= 0
                    ? OpenStaadWrapperHelpers.GetWhenNoRunningInstancesExists(fileFullPath)
                    : OpenStaadWrapperHelpers.GetWhenRunningInstancesExists(fileFullPath);
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Could not acquire an OpenStaad object. Possible reason could be that a running instance of Staad.Pro is not found!" +
                    "In order to create the OpenStaad object, please ensure that the Staad.Pro application is running.", "Get OpenStaad Objects", MessageBoxButtons.OK);
            }

            return new OpenStaadWrapper(null, false);
        }

        #endregion
    }

    public class OpenStaadWrapperHelpers
    {
        public static OpenStaadWrapper GetWhenNoRunningInstancesExists(string fileFullPath)
        {
            OpenSTAAD openStaad = null;
            bool dedicated = false;

            if (string.IsNullOrWhiteSpace(fileFullPath))
            {
                openStaad = OpenStaadHelpers.GetByStartingStaadApplication();
                dedicated = true;
            }
            else
            {
                string directory = Path.GetDirectoryName(fileFullPath);
                if (!(Path.GetDirectoryName(fileFullPath) == null))
                {
                    if (File.Exists(fileFullPath))
                    {
                        openStaad = OpenStaadHelpers.GetByOpeningExistingStaadModel(fileFullPath);
                        dedicated = true;
                    }
                    else
                    {
                        openStaad = OpenStaadHelpers.GetByCreatingBlankStaadModel(fileFullPath);
                        dedicated = true;
                    }
                }
            }

            return new OpenStaadWrapper(openStaad, dedicated);
        }

        public static OpenStaadWrapper GetWhenRunningInstancesExists(string fileFullPath)
        {
            OpenSTAAD openStaad;
            bool dedicated = false;

            if (string.IsNullOrWhiteSpace(fileFullPath))
            {
                openStaad = Marshal.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD;

                string staadFilename = openStaad.GetStaadFileFullPath();
                dedicated = string.IsNullOrWhiteSpace(staadFilename);
            }
            else
            {
                openStaad = OpenStaadHelpers.GetOpenStaadFrom(fileFullPath);

                if (openStaad == null)
                {
                    try
                    {
                        openStaad = OpenStaadHelpers.GetByCreatingBlankStaadModel(fileFullPath);
                        dedicated = true;
                    }
                    catch (Exception ex)
                    {
                        _ = MessageBox.Show($"An error is encounter! Refer to the following exception details.\n{ex.Message}",
                                        "Acquire OpenStaad Object", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    dedicated = true;
                }
            }

            return new OpenStaadWrapper(openStaad, dedicated);
        }
    }
}
