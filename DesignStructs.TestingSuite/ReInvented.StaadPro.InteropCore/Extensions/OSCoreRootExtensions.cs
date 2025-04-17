using System;
using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreRootExtensions
    {
        #region Staad Window Management

        public static void MinimizeStaadMainWindow(this OSCoreRoot openStaad) => openStaad.ComObject.MinimizeStaadMainWindow();

        public static void MaximizeStaadMainWindow(this OSCoreRoot openStaad) => openStaad.ComObject.MaximizeStaadMainWindow();

        public static void RestoreStaadMainWindow(this OSCoreRoot openStaad) => openStaad.ComObject.RestoreStaadMainWindow();

        #endregion

        #region Public Functions

        [Obsolete("Implementation is in progress. Currently deletes only geometry.", false)]
        public static bool DeleteAll(this OSCoreRoot openStaad, int nThreads = 1)
        {
            return openStaad.ComObject.DeleteAll(nThreads);
        }

        public static bool IsTarget(this OSCoreRoot openStaad, string staadFileFullPath)
        {
            return openStaad.ComObject.IsTarget(staadFileFullPath);
        }

        public static bool ModelIsOpen(this OSCoreRoot openStaad, string fileFullPath)
        {
            return openStaad.ComObject.ModelIsOpen(fileFullPath);
        }

        public static void CloseCurrentStaadModel(this OSCoreRoot openStaad) => openStaad.ComObject.CloseCurrentStaadModel();

        public static bool AwaitModelHandle(this OSCoreRoot openStaad, string fileFullPath, double timeoutDurationInSeconds = 60.0, double sleepIntervalInSeconds = 0.1)
        {
            return openStaad.ComObject.AwaitModelHandle(fileFullPath, timeoutDurationInSeconds, sleepIntervalInSeconds);
        }


        public static bool CloseWriteReopenStaadFile(this OSCoreRoot openStaad, string filePath, List<string> fileContents, int sleepTimeMilliseconds = 200, int maxTimeMilliseconds = 5000)
        {
            return openStaad.ComObject.CloseWriteReopenStaadFile(filePath, fileContents, sleepTimeMilliseconds, maxTimeMilliseconds);
        }

        public static string GetStaadFileFullPath(this OSCoreRoot openStaad) => openStaad.ComObject.GetStaadFileFullPath();

        public static string GetAnlFileFullPath(this OSCoreRoot openStaad) => openStaad.ComObject.GetAnlFileFullPath();

        public static string GetStaadFileNameOnly(this OSCoreRoot openStaad) => openStaad.ComObject.GetStaadFileNameOnly();

        public static HashSet<MemberForces> RetrieveMemberForces(this OSCoreRoot openStaad, IEnumerable<int> beams, IEnumerable<int> loadCases)
        {
            return openStaad.ComObject.RetrieveMemberForces(beams, loadCases);
        }

        public static string GetOpenedStaadFilename(this OSCoreRoot openStaad) => openStaad.ComObject.GetOpenedStaadFilename();

        public static bool OpenStadModel(this OSCoreRoot openStaad, string staadFileFullPath, int sleepInterval, int maxWaitTime = 600)
        {
            return openStaad.ComObject.OpenStadModel(staadFileFullPath, sleepInterval, maxWaitTime);
        }

        #endregion
    }
}
