using System;
using System.IO;
using System.Windows;

using OpenSTAADUI;

using ReInvented.Shared.Helpers;
using ReInvented.StaadPro.Interop.Base;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Models;

namespace ReInvented.StaadPro.InteropCore.Helpers
{
    public class OpenStaadWrapperHelpersCore : OpenStaadWrapperHelpersBase
    {
        public static OpenStaadWrapper GetWhenRunningInstancesExists(string fileFullPath)
        {
            OpenSTAAD openStaad; bool dedicated = false;

            if (string.IsNullOrWhiteSpace(fileFullPath))
            {
                ///TODO: This line is the only difference between .NET Framework and .NET Core versions
                openStaad = MarshalHelpers.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD;

                string staadFilename = openStaad.GetStaadFileFullPath();
                dedicated = string.IsNullOrWhiteSpace(staadFilename);
            }
            else
            {
                openStaad = OpenStaadHelpersBase.GetOpenStaadFrom(fileFullPath);

                if (openStaad == null)
                {
                    if (File.Exists(fileFullPath))
                    {
                        openStaad = OpenStaadHelpersBase.GetByOpeningExistingStaadModel(fileFullPath);
                        dedicated = true;
                    }
                    else
                    {
                        try
                        {
                            openStaad = OpenStaadHelpersBase.GetByCreatingBlankStaadModel(fileFullPath);
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
            // OpenStaadWrapper wrapper = new OpenStaadWrapper(openStaad, dedicated);
            // return wrapper.StaadEdition != Enums.ApplicationEdition.Connect ? GetWhenNoRunningInstancesExists(fileFullPath) : wrapper;

            return new OpenStaadWrapper(openStaad, dedicated);
        }
    }
}
