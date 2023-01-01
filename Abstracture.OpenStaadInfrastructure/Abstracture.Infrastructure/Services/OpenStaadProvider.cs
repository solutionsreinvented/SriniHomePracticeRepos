using OpenSTAADUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Abstracture.Infrastructure.Services
{
    public static class OpenStaadProvider
    {
        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

        public static OpenSTAAD GetStaadUiInterface(string staadFilename)
        {
            string lookUpCandidateName = "";/// GetStaadProCandidateNameToLookUpInRot();
            if (string.IsNullOrEmpty(lookUpCandidateName))
            {
                return null;
            }
            if (string.IsNullOrEmpty(staadFilename))
            {
                return null;
            }
            string staadFileFullPath = Path.GetFullPath(staadFilename).ToUpper();

            List<RotObjectInfo> runningObjects = GetRunningObjectList();
            foreach (RotObjectInfo item in runningObjects)
            {
                string candidateName = item.Name.ToUpper();

                try
                {
                    // if this call succeeds, then it is a valid path, else it will continue
                    string extStr = Path.GetExtension(candidateName).ToUpper();
                    {
                        OpenSTAAD osObject = null;
                        if (!string.IsNullOrEmpty(extStr))
                        {
                            // check if it’s a valid STAAD file
                            if (extStr == ".STD")
                            {
                                osObject = item.Value as OpenSTAAD;
                                if (osObject != null)
                                {
                                    // get staad filename
                                    string openedStaadFile = GetOpenedStaadFilename(osObject).ToUpper();
                                    if (string.Compare(staadFileFullPath, openedStaadFile, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        return osObject;
                                    }
                                }
                            }
                        }

                        if (candidateName.StartsWith(lookUpCandidateName))
                        {
                            // we got the staad instance
                            osObject = item.Value as OpenSTAAD;

                            if (osObject != null)
                            {
                                // get staad filename
                                string lookupStaadFilename = GetOpenedStaadFilename(osObject);
                                //
                                if (string.Compare(staadFileFullPath, lookupStaadFilename, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    return osObject;
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

        private static string GetOpenedStaadFilename(OpenSTAAD osObject)
        {
            // get staad filename
            string staadFilename = "";
            bool includeFullPath = true;
            object oArg1 = staadFilename as object;
            object oArg2 = includeFullPath as object;
            osObject.GetSTAADFile(ref oArg1, oArg2);
            staadFilename = oArg1 as string;
            if (staadFilename != null)
                return Path.GetFullPath(staadFilename);
            //
            return null;
        }

        public static List<RotObjectInfo> GetRunningObjectList()
        {
            try
            {
                var result = new List<RotObjectInfo>();

                var numFetched = new IntPtr();
                var monikers = new IMoniker[1];

                GetRunningObjectTable(0, out IRunningObjectTable runningObjectTable);

                runningObjectTable.EnumRunning(out IEnumMoniker monikerEnumerator);
                monikerEnumerator.Reset();

                while (monikerEnumerator.Next(1, monikers, numFetched) == 0)
                {
                    CreateBindCtx(0, out IBindCtx ctx);
                    monikers[0].GetDisplayName(ctx, null, out string runningObjectName);

                    runningObjectTable.GetObject(monikers[0], out object runningObjectVal);

                    var objInfo = new RotObjectInfo
                    {
                        Name = runningObjectName,
                        Value = runningObjectVal
                    };

                    result.Add(objInfo);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }

    public class RotObjectInfo
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
}
