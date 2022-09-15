using SlvParkview.FinanceManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SlvParkview.FinanceManager.Services
{
    public static class ReportsProvider
    {
        public static List<TransactionRecord> GenerateTransactionHistory(Flat flat)
        {
            return GenerateTransactionHistory(flat, DateTime.Today);
        }

        public static List<PrintableTransaction> GetPrintableTransactions(List<TransactionRecord> transactionRecords)
        {
            List<PrintableTransaction> printableTransactions = new List<PrintableTransaction>();

            transactionRecords.ForEach(t => printableTransactions.Add(t.PrintableTransaction));

            return printableTransactions;
        }

        public static List<TransactionRecord> GenerateTransactionHistory(Flat flat, DateTime summarizeTill)
        {
            List<Expense> allExpenses = flat.Expenses?
                                            .Where(e => e.OccuredOn >= flat.AccountOpenedOn && e.OccuredOn <= summarizeTill).ToList();
            List<Payment> allPayments = flat.Payments?
                                            .Where(p => p.ReceivedOn >= flat.AccountOpenedOn && p.ReceivedOn <= summarizeTill).ToList();

            if (allExpenses?.Count <= 0 && allPayments?.Count <= 0)
            {
                return null;
            }

            List<TransactionRecord> transactionRecords = new List<TransactionRecord>();

            DateTime date = flat.AccountOpenedOn;

            decimal outstanding = flat.OpeningBalance;

            while (date <= summarizeTill)
            {
                List<Expense> expensesOnThisDate = allExpenses?.Where(e => e.OccuredOn == date).ToList();
                List<Payment> paymentsOnThisDate = allPayments?.Where(p => p.ReceivedOn == date).ToList();

                int maxCount = Math.Max(expensesOnThisDate == null ? 0 : expensesOnThisDate.Count,
                                        paymentsOnThisDate == null ? 0 : paymentsOnThisDate.Count);

                if (maxCount > 0)
                {
                    int counter = 0;

                    while (counter < maxCount)
                    {

                        TransactionRecord transactionRecord = new TransactionRecord
                        {
                            TransactionDate = date,
                        };

                        if (counter < expensesOnThisDate?.Count)
                        {
                            outstanding += expensesOnThisDate[counter].Amount;

                            transactionRecord.Expense = expensesOnThisDate[counter];
                        }

                        if (counter < paymentsOnThisDate?.Count)
                        {
                            outstanding -= paymentsOnThisDate[counter].Amount;

                            transactionRecord.Payment = paymentsOnThisDate[counter];
                        }

                        transactionRecord.Outstanding = outstanding;

                        transactionRecords.Add(transactionRecord);

                        counter++;
                    }

                }

                date = date.AddDays(1);
            }


            return transactionRecords;
        }
    }
}
