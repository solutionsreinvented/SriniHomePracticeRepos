using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace DesignStructs.FluentValidationExercise.Services
{
    public static class RotHelpers
    {
        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

        public static string GetStaadProCandidateNameToLookUpInRot() => "STAAD";

        public static List<RotObjectInfo> GetRunningObjectList()
        {
            try
            {
                var result = new List<RotObjectInfo>();

                var numFetched = new IntPtr();
                IRunningObjectTable runningObjectTable;
                IEnumMoniker monikerEnumerator;
                var monikers = new IMoniker[1];

                GetRunningObjectTable(0, out runningObjectTable);

                runningObjectTable.EnumRunning(out monikerEnumerator);
                monikerEnumerator.Reset();

                while (monikerEnumerator.Next(1, monikers, numFetched) == 0)
                {
                    IBindCtx ctx;
                    CreateBindCtx(0, out ctx);

                    string runningObjectName;
                    monikers[0].GetDisplayName(ctx, null, out runningObjectName);

                    object runningObjectVal;
                    runningObjectTable.GetObject(monikers[0], out runningObjectVal);

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
}


