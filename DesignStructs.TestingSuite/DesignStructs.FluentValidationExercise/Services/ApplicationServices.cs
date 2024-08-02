using System;
using System.Diagnostics;
using System.Threading;

using DesignStructs.FluentValidationExercise.Models;

namespace DesignStructs.FluentValidationExercise.Services
{
    public static class ApplicationServices
    {
        public static ProcessResult StartApplication(string applicationPath, string windowTitle, int waitTimeInSeconds)
        {
            ProcessResult processResult;

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = applicationPath;
                process.StartInfo.UseShellExecute = true;
                process.Start();

                bool isReady = false;

                for (int i = 0; i < waitTimeInSeconds; i++)
                {
                    Thread.Sleep(1000);
                    process.Refresh();

                    if (process.MainWindowTitle.Contains(windowTitle)) // Adjust according to the actual title
                    {
                        isReady = true;
                        break;
                    }
                }

                processResult = isReady ? new ProcessResult(true, $"{windowTitle} is ready for use.") :
                                          new ProcessResult(true, $"{windowTitle} did not start within the expected time.");
            }
            catch (Exception ex)
            {
                processResult = new ProcessResult(true, $"An error occurred while opening ({windowTitle}): {ex.Message}");
            }

            return processResult;
        }
    }
}
