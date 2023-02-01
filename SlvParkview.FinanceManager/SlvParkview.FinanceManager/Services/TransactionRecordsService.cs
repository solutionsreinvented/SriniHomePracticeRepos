﻿using System;
using System.Collections.Generic;
using System.Linq;

using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;

namespace SlvParkview.FinanceManager.Services
{
    public static class TransactionRecordsService
    {
        public static List<TransactionRecord> Generate(IEnumerable<Receipt> payments)
        {
            if (payments == null) return null;

            IEnumerable<TransactionRecord> records = from p in payments
                                                     select new TransactionRecord() { TransactionDate = p.ReceivedOn, Receipt = p };

            return records.ToList();
        }

        public static List<TransactionRecord> Generate(IEnumerable<Bill> expenses)
        {
            if (expenses == null) return null;

            IEnumerable<TransactionRecord> records = from e in expenses
                                                     select new TransactionRecord() { TransactionDate = e.OccuredOn, Bill = e };

            return records.ToList();
        }

        public static IEnumerable<TransactionRecord> Summarize(IEnumerable<TransactionRecord> records,
                                                               DateTime startDate,
                                                               DateTime endDate,
                                                               decimal openingBalance)
        {
            if (records == null) return null;

            decimal currentOutstanding = openingBalance;

            List<TransactionRecord> summaryRecords = new List<TransactionRecord>();/// = records.OrderBy(r => r.TransactionDate).ThenBy(r => r.Receipt);


            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                IEnumerable<TransactionRecord> paymentsOnThisDate = records.Where(r => r.Receipt != null && r.Receipt.ReceivedOn == currentDate);
                IEnumerable<TransactionRecord> expensesOnThisDate = records.Where(r => r.Bill != null && r.Bill.OccuredOn == currentDate);


                foreach (TransactionRecord p in paymentsOnThisDate)
                {
                    currentOutstanding -= p.Receipt.Amount;
                    p.Outstanding = currentOutstanding;
                    summaryRecords.Add(p);
                }

                foreach (TransactionRecord e in expensesOnThisDate)
                {
                    currentOutstanding += e.Bill.Amount;
                    e.Outstanding = currentOutstanding;
                    summaryRecords.Add(e);
                }

                currentDate = currentDate.AddDays(1);
            }

            return summaryRecords;
        }
    }
}
