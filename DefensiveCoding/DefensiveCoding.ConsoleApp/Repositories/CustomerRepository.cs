using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefensiveCoding.ConsoleApp.Models;

namespace DefensiveCoding.ConsoleApp.Repositories
{
    public class CustomerRepository
    {
        public void Add(Customer customer)
        {
            Console.WriteLine("Processing customer data...");
            Console.WriteLine("Adding customer data to database...");
            Console.WriteLine("Completed adding new customer data!");
        }
    }
}
