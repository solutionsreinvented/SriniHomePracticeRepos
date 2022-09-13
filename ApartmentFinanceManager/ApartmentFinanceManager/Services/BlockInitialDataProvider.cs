using System;
using System.Collections.Generic;

using static ApartmentFinanceManager.Enums.ResidentType;
using ApartmentFinanceManager.Models;

namespace ApartmentFinanceManager.Services
{
    internal static class BlockInitialDataProvider
    {
        public static ApartmentBlock Generate()
        {
            ApartmentBlock block = new ApartmentBlock() { Name = "C" };

            List<Flat> flats = new List<Flat>()
            {
                new Flat(block, "Sandeep A Deshpande") { Number = "017", OpeningBalance = 4600m },
                new Flat(block, "Rajesh Singh") { Number = "018", OpeningBalance = 2800m },
                new Flat(block, "Manjunath") { Number = "019", OpeningBalance = 24240m },
                new Flat(block, "B V Nagaraja") { Number = "020", OpeningBalance = 2800m, ResidentType = Tenant, TenantName = "Manjunath K S" },
                new Flat(block, "Satyanarayana", "Chaya G") { Number = "021", OpeningBalance = 2800m },
                new Flat(block, "Ganapathi M K") { Number = "022", OpeningBalance = 2800m },
                new Flat(block, "B V Chandrashekar") { Number = "023", OpeningBalance = 18208m, ResidentType = Tenant, TenantName = "Raghunath" },
                new Flat(block, "B Mohan Sundaram", "Manika Lakshmi") { Number = "024", OpeningBalance = 2495m, ResidentType = Tenant },
                new Flat(block, "B V Siddalingaswamy") { Number = "117", OpeningBalance = 16800m, ResidentType = Tenant },
                new Flat(block, "B V Prasad") { Number = "118", OpeningBalance = 5600m, ResidentType = Tenant, TenantName = "Santhosh" },
                new Flat(block, "Gangaiah", "Lalitha M R") { Number = "119", OpeningBalance = 14000m },
                new Flat(block, "Shivanand Chawhan") { Number = "120", OpeningBalance = 5600m },
                new Flat(block, "Girish") { Number = "121", OpeningBalance = 2800m },
                new Flat(block, "B J Abhishek", "Bharani K") { Number = "122", OpeningBalance = 2845m },
                new Flat(block, "Arun Joshi") { Number = "123", OpeningBalance = 6239m },
                new Flat(block, "V S Lokeswarappa", "Manoj L") { Number = "124", OpeningBalance = 2800m },
                new Flat(block, "Srinivasa Rao Masanam") { Number = "217", OpeningBalance = 2800m },
                new Flat(block, "Laxmikanth Rao") { Number = "218", OpeningBalance = 2800m },
                new Flat(block, "Nagendra Murthy M", "Shobha B") { Number = "219", OpeningBalance = 5600m },
                new Flat(block, "Somala M", "Gopal N") { Number = "220", OpeningBalance = 20100m },
                new Flat(block, "Bhavani", "Ravi Prakash") { Number = "221", OpeningBalance = 2800m },
                new Flat(block, "B V Chandrashekar") { Number = "222", OpeningBalance = 5600m, ResidentType = Tenant, TenantName = "Harish" },
                new Flat(block, "Praveen Gouda Patil") { Number = "223", OpeningBalance = 1400m },
                new Flat(block, "Pradeepa K S", "Vaishnavi G R") { Number = "224", OpeningBalance = 2800m },
                new Flat(block, "Santosh Kumar P", "Ashwini Krishnappa") { Number = "317", OpeningBalance = 0m },
                new Flat(block, "B V Chandrashekar") { Number = "318", OpeningBalance = 16800m, ResidentType = Tenant },
                new Flat(block, "M G Harish Babu") { Number = "319", OpeningBalance = 18240m },
                new Flat(block, "Shrikanth MH", "Jyothi HC") { Number = "320", OpeningBalance = 2800m },
                new Flat(block, "Santosh P", "Nithya B") { Number = "321", OpeningBalance = 40385m },
                new Flat(block, "Sai Shravanth") { Number = "322", OpeningBalance = 2800m },
                new Flat(block, "Nikhil Kulkarni") { Number = "323", OpeningBalance = 21490m },
                new Flat(block, "B V Nagaraja", "Deepa BN") { Number = "324", OpeningBalance = 2600m, ResidentType = Owner },

            };

            var accountOpenDate = new DateTime(2022, 09, 01);

            block.Flats = flats;

            foreach (var flat in block.Flats)
            {
                flat.AccountOpenedOn = accountOpenDate;
            }


            return block;
        }
    }
}
