using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var repeat = true;
            while (repeat)
            {
                ShowPersonalData();
                //DoCalculations();
                Console.WriteLine("Do you want to repeat this again! [Y/N]: ");
                repeat = (Console.ReadLine() == "Y" || Console.ReadLine() == "y") ? true : false;

            }
        }

        private static void DoCalculations()
        {
            Console.WriteLine("Enter first number:");
            var first = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter second number:");
            var second = int.Parse(Console.ReadLine());

            Console.WriteLine("I have calculated the addition and your result is : " + (first + second));
        }

        private static void ShowPersonalData()
        {
            Console.WriteLine("Enter your name:");
            var name = Console.ReadLine();
            Console.WriteLine("Enter your date of birth:");
            var dob = DateTime.Parse(Console.ReadLine());
            var age = (DateTime.Now - dob).Days / 30 / 12;

            Console.WriteLine("Your name is " + name + " and your age is " + age + " years.");
        }
    }
}

