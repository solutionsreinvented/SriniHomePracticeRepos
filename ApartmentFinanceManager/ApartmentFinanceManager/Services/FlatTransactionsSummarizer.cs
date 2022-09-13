using ApartmentFinanceManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ApartmentFinanceManager.Services
{
    public static class FlatTransactionsSummarizer
    {
        public static List<FlatTransactionRecord> Generate(Flat flat)
        {
            return Generate(flat, DateTime.Today);
        }

        public static List<FlatTransactionRecord> Generate(Flat flat, DateTime summarizeTill)
        {
            if (flat.Expenses == null && flat.Payments == null) return null;

            List<FlatTransactionRecord> transactionRecords = new List<FlatTransactionRecord>();

            DateTime date = flat.AccountOpenedOn;

            while(date <= summarizeTill)
            {
                List<Expense> expenses = flat.Expenses.Where(e => e.OccuredOn == date).ToList();
                List<Payment> payments = flat.Payments.Where(e => e.ReceivedOn == date).ToList();

                decimal outstanding = flat.OpeningBalance;

                if (expenses != null || payments != null )
                {
                    FlatTransactionRecord transactionRecord = new FlatTransactionRecord();
                    transactionRecord.TransactionDate = date;

                    if (expenses != null)
                    {
                        transactionRecord.Expenses = expenses;
                        outstanding += expenses.Sum(e => e.Amount);
                    }

                    if (payments != null)
                    {
                        transactionRecord.Payments = payments;
                        outstanding += payments.Sum(p => p.Amount);
                    }

                    transactionRecord.Outstanding = outstanding;

                }

            }


            return new List<FlatTransactionRecord>();
        }
    }
}
