using SlvParkview.FinanceManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SlvParkview.FinanceManager.Services
{
    public static class FlatTransactionsSummarizer
    {
        public static List<TransactionRecord> Generate(Flat flat)
        {
            return Generate(flat, DateTime.Today);
        }

        public static List<TransactionRecord> Generate(Flat flat, DateTime summarizeTill)
        {
            if (flat.Expenses == null && flat.Payments == null)
            {
                return null;
            }

            List<TransactionRecord> transactionRecords = new List<TransactionRecord>();

            DateTime date = flat.AccountOpenedOn;

            decimal outstanding = flat.OpeningBalance;

            while (date <= summarizeTill)
            {
                List<Expense> expenses = flat.Expenses?.Where(e => e.OccuredOn == date).ToList();
                List<Payment> payments = flat.Payments?.Where(e => e.ReceivedOn == date).ToList();

                if ((expenses != null && expenses.Count > 0) || (payments != null && payments.Count > 0))
                {
                    TransactionRecord transactionRecord = new TransactionRecord
                    {
                        TransactionDate = date
                    };

                    if (expenses != null && expenses.Count > 0)
                    {
                        transactionRecord.Expenses = expenses;
                        outstanding += expenses.Sum(e => e.Amount);
                    }

                    if (payments != null && payments.Count > 0)
                    {
                        transactionRecord.Payments = payments;
                        outstanding -= payments.Sum(p => p.Amount);
                    }

                    transactionRecord.Outstanding = outstanding;

                    transactionRecords.Add(transactionRecord);
                }

                date = date.AddDays(1);
            }


            return transactionRecords;
        }
    }
}
