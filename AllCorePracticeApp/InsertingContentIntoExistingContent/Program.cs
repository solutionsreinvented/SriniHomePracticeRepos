using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace InsertingContentIntoExistingContent
{
    class Program
    {
        static void Main(string[] args)
        {

            FileContentManager.GenerateShortSyntax();

            List<string> strings = new List<string>()
            {
                "STAAD SPACE",
                "MY NAME",
                "YOUR NAME",
                "LOAD R34 TYPE DEAD WATER PRESSURE",
                "SOME JUNK",
                "SOME MORE JUNK",
                "LOAD R61 TYPE SEISMIC IMPULSIVE",
                "NEW JUNK",
                "NEW MORE JUNK",
                "FINISH"
            };
            int startMarkerIndex = strings.FindIndex(l => l.Contains("Load R34".ToUpperInvariant()));
            int endMarkerIndex = strings.FindIndex(l => l.Contains("Load R61".ToUpperInvariant()));

            List<string> newJunk = new List<string>()
            {
                "REPLACED JUNK",
                "MORE REPLACED JUNK"
            };

            List<string> updatedContent = new List<string>();
            updatedContent.AddRange(strings.Take(startMarkerIndex + 1));
            updatedContent.AddRange(newJunk);
            updatedContent.AddRange(strings.Skip(endMarkerIndex));
        }
    }

    public class FileContentManager
    {
        public static void GenerateShortSyntax()
        {
            int[] numbers = { 1, 2, 3, 8, 7, 5, 23, 43, 44, 45, 46, 47, 48 };

            Array.Sort(numbers); // Sort the array in ascending order

            var result = GenerateShortStatements(numbers);

            Console.WriteLine(result);
        }

        static IEnumerable<string> GenerateShortStatements(int[] numbers)
        {
            //string result = "";

            //int start = numbers[0];
            //int end = numbers[0];

            //for (int i = 1; i < numbers.Length; i++)
            //{
            //    if (numbers[i] == end + 1)
            //    {
            //        end = numbers[i];
            //    }
            //    else
            //    {
            //        if (end - start >= 2) // Check if there are 3 or more sequential numbers
            //        {
            //            result += $"{start} TO {end} ";
            //        }
            //        else
            //        {
            //            result += $"{start} ";
            //            if (start != end)
            //            {
            //                result += $"{end} ";
            //            }
            //        }
            //        start = end = numbers[i];
            //    }
            //}

            //// Handle the last block
            //if (end - start >= 2) // Check if there are 3 or more sequential numbers
            //{
            //    result += $"{start} TO {end}";
            //}
            //else
            //{
            //    result += $"{start} ";
            //    if (start != end)
            //    {
            //        result += $"{end}";
            //    }
            //}

            //return result;

            var characterLimit = 7;

            List<string> result = new List<string>();

            int start = numbers[0];
            int end = numbers[0];
            string currentLine = $"{start}";

            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] == end + 1)
                {
                    end = numbers[i];
                }
                else
                {
                    if (end - start >= 2) // Check if there are 3 or more sequential numbers
                    {
                        currentLine += $" TO {end}";
                    }
                    else
                    {
                        currentLine += end != start ? $" {end}" : "";
                    }

                    if (result.Count > 0 && result.Last().Length + currentLine.Length + 1 > characterLimit)
                    {
                        result[result.Count - 1] += " -";
                    }
                    else
                    {
                        result.Add(result.Count > 0 ? $" {currentLine}" : currentLine);
                    }

                    start = end = numbers[i];
                    currentLine = $"{start}";
                }
            }

            // Handle the last block
            if (end - start >= 2) // Check if there are 3 or more sequential numbers
            {
                currentLine += $" TO {end}";
            }
            else
            {
                currentLine += end != start ? $" {end}" : "";
            }

            if (result.Count > 0 && result.Last().Length + currentLine.Length + 1 > characterLimit)
            {
                result[result.Count - 1] += " -";
            }
            else
            {
                result.Add(result.Count > 0 ? $" {currentLine}" : currentLine);
            }

            return result;

        }

    }
}
