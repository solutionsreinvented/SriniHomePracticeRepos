using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

using OpenSTAADUI;

using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Models;

namespace ReInvented.StaadPro.InteropCore.Helpers
{
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
                ///openStaad = Marshal.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD;
                openStaad = OpenStaadHelpers.GetOpenStaadFrom(fileFullPath);

                string staadFilename = openStaad.GetStaadFileFullPath();
                dedicated = string.IsNullOrWhiteSpace(staadFilename);
            }
            else
            {
                openStaad = OpenStaadHelpers.GetOpenStaadFrom(fileFullPath);

                if (openStaad == null)
                {
                    if (File.Exists(fileFullPath))
                    {
                        openStaad = OpenStaadHelpers.GetByOpeningExistingStaadModel(fileFullPath);
                        dedicated = true;
                    }
                    else
                    {
                        try
                        {
                            openStaad = OpenStaadHelpers.GetByCreatingBlankStaadModel(fileFullPath);
                            dedicated = true;
                        }
                        catch (Exception ex)
                        {
                            _ = MessageBox.Show($"An error is encountered! Refer to the following exception details.\n{ex.Message}",
                                            "Acquire OpenStaad Object", MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    dedicated = true;
                }
            }

            // Below code is for testing. May not be working correctly!
            //OpenStaadWrapper wrapper = new OpenStaadWrapper(openStaad, dedicated);
            //return wrapper.StaadEdition != Enums.ApplicationEdition.Connect ? GetWhenNoRunningInstancesExists(fileFullPath) : wrapper;

            return new OpenStaadWrapper(openStaad, dedicated);
        }
    }
}
