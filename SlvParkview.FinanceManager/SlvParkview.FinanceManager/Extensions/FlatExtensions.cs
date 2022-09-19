﻿using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting;
using SlvParkview.FinanceManager.Reporting.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SlvParkview.FinanceManager.Extensions
{
    public static class FlatExtensions
    {
        #region Get Printable Transactions

        /// <summary>
        ///  Generates date wise transactions with only basic information required for preparing a tabular information.
        /// </summary>
        /// <param name="transactionRecords">A <see cref="List{TransactionRecord}"/> from which the summary to be prepared.</param>
        /// <returns>A <see cref="List{PrintableTransaction}"/> to facilitate the preparation of an html table.</returns>
        public static List<TransactionInfo> GetTransactionsHistoryBasic(this Flat flat)
        {
            return GetTransactionsHistoryBasic(flat, DateTime.Today);
        }

        /// <summary>
        ///  Generates date wise transactions with only basic information required for preparing a tabular information.
        /// </summary>
        /// <param name="flat">Flat for which the transactions summary to be prepared.</param>
        /// <param name="summarizeTill">Date up to which the transactions to be considered.</param>
        /// <returns>A <see cref="List{PrintableTransaction}"/> to facilitate the preparation of an html table.</returns>
        public static List<TransactionInfo> GetTransactionsHistoryBasic(this Flat flat, DateTime summarizeTill)
        {
            return GetTransactionsHistory(GetTransactionsHistoryDetailed(flat, summarizeTill));
        }

        /// <summary>
        ///  Generates date wise transactions with only basic information required for preparing a tabular information.
        /// </summary>
        /// <param name="transactionRecords">A <see cref="List{TransactionRecord}"/> from which the summary to be prepared.</param>
        /// <returns>A <see cref="List{PrintableTransaction}"/> to facilitate the preparation of an html table.</returns>
        public static List<TransactionInfo> GetTransactionsHistory(List<TransactionRecord> transactionRecords)
        {
            List<TransactionInfo> printableTransactions = new List<TransactionInfo>();

            transactionRecords.ForEach(t => printableTransactions.Add(t.TransactionInfo));

            return printableTransactions;
        }

        #endregion

        #region Get Transactions

        /// <summary>
        /// Generates date wise transactions with full <see cref="Expense"/> and <see cref="Payment"/> information up to this date.
        /// </summary>
        /// <param name="flat">Flat for which the transactions summary to be prepared.</param>
        /// <returns>A <see cref="List{TransactionRecord}"/> with full <see cref="Expense"/> and <see cref="Payment"/> details summarized date wise.</returns>
        public static List<TransactionRecord> GetTransactionsHistoryDetailed(this Flat flat)
        {
            return GetTransactionsHistoryDetailed(flat, DateTime.Today);
        }

        /// <summary>
        /// Generates date wise transactions with full <see cref="Expense"/> and <see cref="Payment"/> information.
        /// </summary>
        /// <param name="flat">Flat for which the transactions summary to be prepared.</param>
        /// <param name="summarizeTill">Date up to which the transactions to be considered.</param>
        /// <returns>A <see cref="List{TransactionRecord}"/> with full <see cref="Expense"/> and <see cref="Payment"/> details summarized date wise.</returns>
        public static List<TransactionRecord> GetTransactionsHistoryDetailed(this Flat flat, DateTime summarizeTill)
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

        #endregion

        #region Parsers

        public static FlatInfo ParseToFlatInfo(this Flat flat)
        {
            string dateFormat = "dd MMM yyyy";

            return new FlatInfo()
            {
                AccountOpenedOn = flat.AccountOpenedOn.ToString(dateFormat),
                CoOwnedBy = flat.CoOwnedBy ?? "-",
                CurrentOutstanding = flat.CurrentOutstanding.FormatNumber("N2"),
                DateSpecified = flat.DateSpecified.ToString(dateFormat),
                Description = flat.Description,
                Number = flat.Number.ToString(),
                OpeningBalance = flat.OpeningBalance.FormatNumber("N2"),
                OutstandingOnSpecifiedDate = flat.OutstandingOnSpecifiedDate.FormatNumber("N2"),
                OwnedBy = flat.OwnedBy,
                ResidentType = flat.ResidentType.ToString(),
                TenantName = flat.TenantName ?? "-"
            };
        }

        #endregion
    }
}
