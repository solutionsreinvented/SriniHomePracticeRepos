using System;
using System.Collections.Generic;
using System.Linq;

using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;

namespace SlvParkview.FinanceManager.Services
{
    public static class TransactionRecordsService
    {
        public static List<TransactionRecord> Generate(IEnumerable<Payment> payments)
        {
            if (payments == null) return null;

            IEnumerable<TransactionRecord> records = from p in payments
                                                     select new TransactionRecord() { TransactionDate = p.ReceivedOn, Payment = p };

            return records.ToList();
        }

        public static List<TransactionRecord> Generate(IEnumerable<Expense> expenses)
        {
            if (expenses == null) return null;

            IEnumerable<TransactionRecord> records = from e in expenses
                                                     select new TransactionRecord() { TransactionDate = e.OccuredOn, Expense = e };

            return records.ToList();
        }

        public static IEnumerable<TransactionRecord> Summarize(IEnumerable<TransactionRecord> records,
                                                               DateTime startDate,
                                                               DateTime endDate,
                                                               decimal openingBalance)
        {
            if (records == null) return null;

            decimal currentOutstanding = openingBalance;

            List<TransactionRecord> summaryRecords = new List<TransactionRecord>();/// = records.OrderBy(r => r.TransactionDate).ThenBy(r => r.Payment);


            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                IEnumerable<TransactionRecord> paymentsOnThisDate = records.Where(r => r.Payment != null && r.Payment.ReceivedOn == currentDate);
                IEnumerable<TransactionRecord> expensesOnThisDate = records.Where(r => r.Expense != null && r.Expense.OccuredOn == currentDate);


                foreach (TransactionRecord p in paymentsOnThisDate)
                {
                    currentOutstanding -= p.Payment.Amount;
                    p.Outstanding = currentOutstanding;
                    summaryRecords.Add(p);
                }

                foreach (TransactionRecord e in expensesOnThisDate)
                {
                    currentOutstanding += e.Expense.Amount;
                    e.Outstanding = currentOutstanding;
                    summaryRecords.Add(e);
                }

                currentDate = currentDate.AddDays(1);
            }

            return summaryRecords;
        }
    }
}
