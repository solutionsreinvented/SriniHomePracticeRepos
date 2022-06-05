using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefensiveCoding.ConsoleApp.Models
{
    public class Customer
    {


        public Customer()
        {

        }

        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string PrintDetails()
        {
            return string.Concat("Id: ", CustomerId, " First name: ", FirstName, " Last name: ", LastName);
        }

        public string Print(Customer customer)
        {
            return customer.PrintDetails();
        }
    }
}
