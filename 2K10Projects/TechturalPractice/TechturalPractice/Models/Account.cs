using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechturalPractice.Models
{
    public class Account
    {
        public decimal Balance { get; private set; }

        private bool IsVerified { get; set; }
        private bool IsClosed { get; set; }

        public void Deposit(decimal amount)
        {
            if (IsClosed)
                return; /// Or do something else...
            Balance += amount;
        
        }
        public void Withdraw(decimal amount)
        {
            if (!IsVerified)
                return; /// Or do something else...
            if (IsClosed)
                return; /// Or do something else..
            Balance -= amount;
        }

        public void HolderVerified()
        {
            IsVerified = true;
        }

        public void Close()
        {
            IsClosed = true;
        }
    }
}
