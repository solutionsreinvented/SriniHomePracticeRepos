
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace SPro2023ConsoleApp
{
    public class FileRenamer
    {
        public static void RenameFilesIn(string directoryPath)
        {
            try
            {
                //DirectoryInfo directory = new DirectoryInfo(directoryPath);
                var files = Directory.GetFiles(directoryPath);

                foreach (var file in files)
                {
                    string originalFileName = Path.GetFileName(file);
                    string newFileName = RenameFile(originalFileName);
                    string newPath = Path.Combine(directoryPath, newFileName);

                    File.Move(Path.GetFullPath(file), newPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static string RenameFile(string originalFileName)
        {
            // Extracting components using regular expressions
            Match match = Regex.Match(originalFileName, @"^(\d{1,3})\.\s*([A-Za-z]{3,})\s*(\d{4})(.*)$");

            if (match.Success)
            {
                int number = int.Parse(match.Groups[1].Value);
                string monthAbbreviation = match.Groups[2].Value;
                int year = int.Parse(match.Groups[3].Value);
                string remainingText = match.Groups[4].Value;

                // Apply the specified rules
                string formattedNumber = (number < 10) ? $"00{number}" : (number < 100) ? $"0{number}" : number.ToString();
                if (monthAbbreviation.Length > 3)
                {
                    monthAbbreviation = monthAbbreviation.Substring(0, 3);
                }
                string monthName = new DateTime(year, DateTime.ParseExact(monthAbbreviation, "MMM", CultureInfo.InvariantCulture).Month, 1).ToString("MMMM");

                // Construct the new file name
                string newFileName = $"{formattedNumber}. {monthName} {year}{remainingText}";
                return newFileName;
            }

            // If the regular expression doesn't match, return the original name
            return originalFileName;
        }

    }
}
