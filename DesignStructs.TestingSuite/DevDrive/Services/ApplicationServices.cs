using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using OpenSTAADUI;

using ReInvented.Shared.Interfaces;
using ReInvented.Shared.Models;

namespace DevDrive.Services
{
    public static class ApplicationServices
    {
        public static IResult<string> StartApplication(string applicationPath, string windowTitle, int waitTimeSeconds)
        {
            IResult<string> result;

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = applicationPath;
                process.StartInfo.UseShellExecute = true;
                process.Start();

                bool isReady = false;

                for (int i = 0; i < waitTimeSeconds; i++)
                {
                    Thread.Sleep(1000);
                    process.Refresh();

                    if (process.MainWindowTitle.ToLowerInvariant().Contains(windowTitle.ToLowerInvariant())) // Adjust according to the actual title
                    {
                        isReady = true;
                        break;
                    }
                }

                result = isReady ? new Result<string>($"{windowTitle} is ready for use.", true) :
                                          new Result<string>($"{windowTitle} did not start within the expected time.", true);
            }
            catch (Exception ex)
            {
                result = new Result<string>($"An error occurred while opening ({windowTitle}): {ex.Message}", true);
            }

            return result;
        }

        //public static ProcessResult StartApplication(string applicationPath, string windowTitle, int waitTimeSeconds)
        //{
        //    ProcessResult processResult;

        //    try
        //    {
        //        Process process = new Process();
        //        process.StartInfo.FileName = applicationPath;
        //        process.StartInfo.UseShellExecute = true;
        //        process.Start();

        //        bool isReady = false;
                
        //        for (int i = 0; i < waitTimeSeconds; i++)
        //        {
        //            Thread.Sleep(1000);
        //            process.Refresh();

        //            if (process.MainWindowTitle.Contains(windowTitle)) // Adjust according to the actual title
        //            {
        //                isReady = true;
        //                break;
        //            }
        //        }

        //        processResult = isReady ? new ProcessResult(true, $"{windowTitle} is ready for use.") :
        //                                  new ProcessResult(true, $"{windowTitle} did not start within the expected time.");
        //    }
        //    catch (Exception ex)
        //    {
        //        processResult = new ProcessResult(true, $"An error occurred while opening ({windowTitle}): {ex.Message}");
        //    }

        //    return processResult;
        //}
    }
}
