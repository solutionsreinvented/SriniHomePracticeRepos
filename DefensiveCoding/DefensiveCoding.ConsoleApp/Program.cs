using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefensiveCoding.ConsoleApp.Models;
using DefensiveCoding.ConsoleApp.Repositories;

namespace DefensiveCoding.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.SetWindowPosition(100, 100);
            var customer = new Customer();
            var customer2 = new Customer();

            customer2.CustomerId = 2;
            customer2.FirstName = "Srinivas";
            customer2.FirstName = "Masanam";

            var customerRepo = new CustomerRepository();

            customerRepo.Add(customer);
            Console.WriteLine(customer.PrintDetails());
            Console.WriteLine(customer.Print(customer2));


            Console.ReadLine();



        }
    }
}
