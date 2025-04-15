using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

using OpenSTAADUI;

using ReInvented.DataAccess.Services;
using ReInvented.Shared.Extensions;
using ReInvented.Shared.Helpers;
using ReInvented.Shared.Interfaces;
using ReInvented.Shared.Models;
using ReInvented.Shared.Services;
using ReInvented.StaadPro.Interop.Extensions;

namespace ReInvented.StaadPro.InteropCore.Helpers
{
    public class OpenStaadHelpers
    {
        #region Public Static Functions

        public static OpenSTAAD GetOpenStaadFrom(string staadFilename)
        {
            string lookUpCandidateName = RotHelpers.GetStaadProCandidateNameToLookUpInRot();

            if (string.IsNullOrEmpty(lookUpCandidateName) || string.IsNullOrEmpty(staadFilename))
            {
                return null;
            }

            string staadFileFullPath = Path.GetFullPath(staadFilename).ToUpper();

            List<RotItem> runningItems = RotHelpers.GetRunningObjectList();

            foreach (RotItem item in runningItems)
            {
                try
                {
                    if (item.Matches(FileExtensions.StaadApplication, lookUpCandidateName))
                    {
                        if (item.Value is OpenSTAAD openStaad && openStaad.IsTarget(staadFileFullPath))
                        {
                            return openStaad;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"An issue encountered.\n{ex.Message}", "Get OpenStaad");
                }
            }

            return null;
        }

        public static OpenSTAAD GetByStartingStaadApplication(int waitSeconds = 600)
        {
            string applicationPath = @"C:\Program Files\Bentley\Engineering\STAAD.Pro 2024\STAAD\Bentley.Staad.exe";
            IResult<string> processResult = ProcessesService.StartApplication(applicationPath, RegexPatterns.StaadWindowTitle, waitSeconds);

            return processResult.Success ? MarshalHelpers.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD : null;
        }

        public static OpenSTAAD GetByOpeningExistingStaadModel(string withStaadFileFullPath, int waitSeconds = 600)
        {
            IResult<string> processResult = ProcessesService.StartProcess(withStaadFileFullPath, waitSeconds);

            return processResult.Success ? GetOpenStaadFrom(withStaadFileFullPath) : null;
        }

        public static OpenSTAAD GetByCreatingBlankStaadModel(string withStaadFileFullPath, int waitSeconds = 600)
        {
            File.WriteAllLines(withStaadFileFullPath, StaadNewFileDefaultContent());
            return GetByOpeningExistingStaadModel(withStaadFileFullPath, waitSeconds);
        }

        #endregion

        #region Private Helpers

        private static IEnumerable<string> StaadNewFileDefaultContent()
        {
            return new List<string>()
            {
                "STAAD SPACE",
                "START JOB INFORMATION",
                $"ENGINEER DATE {DateTime.Today:dd-MMM-yy}",
                "END JOB INFORMATION",
                "INPUT WIDTH 79",
                "FINISH"
            };
        }

        #endregion

    }
}
