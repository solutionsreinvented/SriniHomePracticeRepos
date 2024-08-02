using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using OpenSTAADUI;

using ReInvented.DataAccess.Services;
using ReInvented.StaadPro.Interactivity.Extensions;

namespace DesignStructs.FluentValidationExercise.Services
{
    public static class MultipleInstanceOpenStaadProvider
    {
        public static OpenSTAAD GetEmptyInstance()
        {
            return null;
        }

        public static OpenSTAAD GetOpenStaadObject(string staadFilename)
        {
            Process.Start(staadFilename);

            IEnumerable<RotObjectInfo> openStaadObjects = RotHelpers.GetRunningObjectList().Where(ro => (ro.Value as OpenSTAAD) != null).Distinct();

            foreach (RotObjectInfo item in openStaadObjects)
            {
                string candidateName = item.Name.ToUpper();
                string osFile = (item.Value as OpenSTAAD).GetStaadFileFullPath().ToUpper();


                try
                {
                    string extStr = Path.GetExtension(candidateName).ToUpper();

                    if (string.IsNullOrEmpty(extStr) && item.Value is OpenSTAAD openStaad)
                    {
                        string openedStaadFile = openStaad.GetStaadFileFullPath().ToUpper();

                        if (string.IsNullOrEmpty(openedStaadFile) && openedStaadFile == Path.GetFullPath(staadFilename))
                        {
                            return openStaad;
                        }
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }

            return null;
        }


        public static OpenSTAAD GetStaadUiInterface(string staadFilename)
        {
            var processes = Process.GetProcesses().Where(p => p.ProcessName == "Bentley.Staad");

            string lookUpCandidateName = RotHelpers.GetStaadProCandidateNameToLookUpInRot();

            //if (string.IsNullOrEmpty(lookUpCandidateName))
            //{
            //    return null;
            //}
            //if (string.IsNullOrEmpty(staadFilename))
            //{
            //    return null;
            //}
            //string staadFileFullPath = Path.GetFullPath(staadFilename).ToUpper();

            string staadFileFullPath = "";

            IEnumerable<RotObjectInfo> openStaadObjects = RotHelpers.GetRunningObjectList().Where(ro => (ro.Value as OpenSTAAD) != null);

            foreach (RotObjectInfo item in openStaadObjects)
            {
                string candidateName = item.Name.ToUpper();

                try
                {
                    // if this call succeeds, then it is a valid path, else it will continue
                    string extStr = Path.GetExtension(candidateName).ToUpper();
                    {
                        OpenSTAAD openStaad = null;
                        if (!string.IsNullOrEmpty(extStr))
                        {
                            // check if it’s a valid STAAD file
                            if (extStr == $".{FileExtensions.StaadApplication.ToUpper()}")
                            {
                                openStaad = item.Value as OpenSTAAD;
                                if (openStaad != null)
                                {
                                    // get staad filename
                                    string openedStaadFile = openStaad.GetStaadFileFullPath().ToUpper();
                                    if (string.Compare(staadFileFullPath, openedStaadFile, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        return openStaad;
                                    }
                                }
                            }
                        }

                        if (candidateName.StartsWith(lookUpCandidateName))
                        {
                            // we got the staad instance
                            openStaad = item.Value as OpenSTAAD;

                            if (openStaad != null)
                            {
                                // get staad filename
                                string lookupStaadFilename = openStaad.GetStaadFileFullPath();
                                //
                                if (string.Compare(staadFileFullPath, lookupStaadFilename, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    return openStaad;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return null;
        }
    }
}
