using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlipkartCodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {


            string filePath = @"C:\Users\srini\Desktop\On Screen\IT\OD433493703500823100.pdf";
            DateTime created = new DateTime(2025, 01, 28, 23, 15, 09);  // Example date
            DateTime modified = new DateTime(2025, 01, 28, 23, 15, 09); // Example date

            SetPdfFileDates(filePath, created, modified);


            //var loop = 'Y';

            //while (loop == 'Y' || loop == 'y')
            //{
            //    var code = GenerateRandomCode(5, 10);
            //    Console.WriteLine(code);
            //    Console.WriteLine("Do you want to generate one more [Y/N]?");

            //    loop = Console.ReadLine().First();
            //}

        }

        static void SetPdfFileDates(string filePath, DateTime created, DateTime modified)
        {
            if (File.Exists(filePath))
            {
                File.SetCreationTime(filePath, created);
                File.SetLastWriteTime(filePath, modified);
            }
            else
            {
                Console.WriteLine("File not found: " + filePath);
            }
        }

        static string GenerateRandomCode(int nChars, int nDigits)
        {
            Random random = new Random();
            char[] code = new char[nChars+nDigits];

            // Generate first three characters as uppercase letters
            for (int i = 0; i < nChars; i++)
            {
                code[i] = (char)random.Next('A', 'Z' + 1);
            }

            // Generate remaining 13 characters as digits
            for (int i = nChars; i < (nChars + nDigits); i++)
            {
                code[i] = (char)random.Next('0', '9' + 1);
            }

            return new string(code);
        }
    }
}
