using System;
using System.Collections.Generic;

using static ApartmentFinanceManager.Enums.ResidentType;
using ApartmentFinanceManager.Models;

namespace ApartmentFinanceManager.Services
{
    internal static class GenerateBlockInitialData
    {
        public static Block Generate()
        {
            var block = new Block() { Name = "C" };

            List<Flat> flats = new List<Flat>()
            {
                new Flat(block, "Sandeep A Deshpande") {Number = 017, OpeningBalance = 2800m},
                new Flat(block, "Rajesh Singh") {Number = 018, OpeningBalance = 2800m},
                new Flat(block, "Manjunath") {Number = 019, OpeningBalance = 2800m},
                new Flat(block, "B V Nagaraja") {Number = 020, OpeningBalance = 2800m, ResidentType = Tenant, TenantName = "Manjunath K S"},
                new Flat(block, "Satyanarayana", "Chaya G") {Number = 021, OpeningBalance = 2800m},
                new Flat(block, "Ganapathi M K") {Number = 022, OpeningBalance = 2800m},
                new Flat(block, "B V Chandrashekar") {Number = 023, OpeningBalance = 2800m, ResidentType = Tenant, TenantName = "Raghunath"},
                new Flat(block, "B Mohan Sundaram", "Manika Lakshmi") {Number = 024, OpeningBalance = 2800m, ResidentType = Tenant},
                new Flat(block, "B V Siddalingaswamy") {Number = 117, OpeningBalance = 2800m, ResidentType = Tenant},
                new Flat(block, "B V Prasad") {Number = 118, OpeningBalance = 2800m, ResidentType = Tenant, TenantName = "Santhosh"},
                new Flat(block, "Gangaiah", "Lalitha M R") {Number = 119, OpeningBalance = 2800m},
                new Flat(block, "Shivanand Chawhan") {Number = 120, OpeningBalance = 2800m},
                new Flat(block, "Girish") {Number = 121, OpeningBalance = 2800m},
                new Flat(block, "B J Abhishek", "Bharani K") {Number = 122, OpeningBalance = 2800m},
                new Flat(block, "Arun Joshi") {Number = 123, OpeningBalance = 2800m},
                new Flat(block, "V S Lokeswarappa", "Manoj L") {Number = 124, OpeningBalance = 2800m},
                new Flat(block, "Srinivasa Rao Masanam") {Number = 217, OpeningBalance = 2800m},
                new Flat(block, "Laxmikanth Rao") {Number = 218, OpeningBalance = 2800m},
                new Flat(block, "Nagendra Murthy M", "Shobha B") {Number = 219, OpeningBalance = 2800m},
                new Flat(block, "Somala M", "Gopal N") {Number = 220, OpeningBalance = 2800m},
                new Flat(block, "Bhavani", "Ravi Prakash") {Number = 221, OpeningBalance = 2800m},
                new Flat(block, "B V Chandrashekar") {Number = 222, OpeningBalance = 2800m, ResidentType = Tenant, TenantName = "Harish"},
                new Flat(block, "Praveen Gouda Patil") {Number = 223, OpeningBalance = 2800m},
                new Flat(block, "Pradeepa K S", "Vaishnavi G R") {Number = 224, OpeningBalance = 2800m},
                new Flat(block, "Santosh Kumar P", "Ashwini Krishnappa") {Number = 317, OpeningBalance = 2800m},
                new Flat(block, "B V Chandrashekar") {Number = 318, OpeningBalance = 2800m, ResidentType = Tenant},
                new Flat(block, "M G Harish Babu") {Number = 319, OpeningBalance = 2800m},
                new Flat(block, "Shrikanth MH", "Jyothi HC") {Number = 320, OpeningBalance = 2800m},
                new Flat(block, "Santosh P", "Nithya B") {Number = 321, OpeningBalance = 2800m},
                new Flat(block, "Sai Shravanth") {Number = 322, OpeningBalance = 2800m},
                new Flat(block, "Nikhil Kulkarni") {Number = 323, OpeningBalance = 2800m},
                new Flat(block, "B V Nagaraja", "Deepa BN") {Number = 324, OpeningBalance = 2800m, ResidentType = Owner},

            };


            return new Block();
        }
    }
}
