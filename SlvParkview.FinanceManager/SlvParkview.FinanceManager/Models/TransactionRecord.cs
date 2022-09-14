using System;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Models
{
    public class TransactionRecord
    {
        public DateTime TransactionDate { get; set; }

        public List<Expense> Expenses { get; set; }

        public List<Payment> Payments { get; set; }

        public decimal Outstanding { get; set; }
    }
}
