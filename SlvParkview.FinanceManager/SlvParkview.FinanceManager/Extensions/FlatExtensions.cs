using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Services;

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
            return GetTransactionsHistory(GetTransactionsHistoryDetailedModified(flat, summarizeTill));
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
        /// Generates date wise transactions with full <see cref="Bill"/> and <see cref="Receipt"/> information up to this date.
        /// </summary>
        /// <param name="flat">Flat for which the transactions summary to be prepared.</param>
        /// <returns>A <see cref="List{TransactionRecord}"/> with full <see cref="Bill"/> and <see cref="Receipt"/> details summarized date wise.</returns>
        public static List<TransactionRecord> GetTransactionsHistoryDetailed(this Flat flat)
        {
            return GetTransactionsHistoryDetailed(flat, DateTime.Today);
        }

        /// <summary>
        /// Generates date wise transactions with full <see cref="Bill"/> and <see cref="Receipt"/> information.
        /// </summary>
        /// <param name="flat">Flat for which the transactions summary to be prepared.</param>
        /// <param name="summarizeTill">Date up to which the transactions to be considered.</param>
        /// <returns>A <see cref="List{TransactionRecord}"/> with full <see cref="Bill"/> and <see cref="Receipt"/> details summarized date wise.</returns>
        public static List<TransactionRecord> GetTransactionsHistoryDetailed(this Flat flat, DateTime summarizeTill)
        {
            List<Bill> allBills = flat.Bills?
                                            .Where(e => e.OccuredOn >= flat.AccountOpenedOn && e.OccuredOn <= summarizeTill).ToList();
            List<Receipt> allReceipts = flat.Receipts?
                                            .Where(p => p.ReceivedOn >= flat.AccountOpenedOn && p.ReceivedOn <= summarizeTill).ToList();

            if (allBills?.Count <= 0 && allReceipts?.Count <= 0)
            {
                return null;
            }

            List<TransactionRecord> transactionRecords = new List<TransactionRecord>();

            DateTime date = flat.AccountOpenedOn;

            decimal outstanding = flat.OpeningBalance;

            while (date <= summarizeTill)
            {
                List<Bill> billsOnThisDate = allBills?.Where(e => e.OccuredOn == date).ToList();
                List<Receipt> receiptsOnThisDate = allReceipts?.Where(p => p.ReceivedOn == date).ToList();

                int maxCount = Math.Max(billsOnThisDate == null ? 0 : billsOnThisDate.Count,
                                        receiptsOnThisDate == null ? 0 : receiptsOnThisDate.Count);

                if (maxCount > 0)
                {
                    int counter = 0;

                    while (counter < maxCount)
                    {

                        TransactionRecord transactionRecord = new TransactionRecord
                        {
                            TransactionDate = date,
                        };

                        if (counter < billsOnThisDate?.Count)
                        {
                            outstanding += billsOnThisDate[counter].Amount;

                            transactionRecord.Bill = billsOnThisDate[counter];
                        }

                        if (counter < receiptsOnThisDate?.Count)
                        {
                            outstanding -= receiptsOnThisDate[counter].Amount;

                            transactionRecord.Receipt = receiptsOnThisDate[counter];
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

        public static List<TransactionRecord> GetTransactionsHistoryDetailedModified(this Flat flat, DateTime summarizeTill)
        {
            List<Bill> billsInRange = flat.Bills?
                                                .Where(e => e.OccuredOn >= flat.AccountOpenedOn && e.OccuredOn <= summarizeTill).ToList();
            List<Receipt> receiptsInRange = flat.Receipts?
                                                .Where(p => p.ReceivedOn >= flat.AccountOpenedOn && p.ReceivedOn <= summarizeTill).ToList();
            List<Bill> penaltiesInRange = flat.Penalties?
                                                 .Where(p => p.OccuredOn >= flat.AccountOpenedOn && p.OccuredOn <= summarizeTill).ToList();

            if (billsInRange?.Count <= 0 && receiptsInRange?.Count <= 0 && penaltiesInRange?.Count <= 0)
            {
                return null;
            }

            List<TransactionRecord> transactionRecords = new List<TransactionRecord>();

            if (TransactionRecordsService.Generate(billsInRange) != null)
            {
                transactionRecords.AddRange(TransactionRecordsService.Generate(billsInRange));
            }
            if (TransactionRecordsService.Generate(penaltiesInRange) != null)
            {
                transactionRecords.AddRange(TransactionRecordsService.Generate(penaltiesInRange));
            }
            if (TransactionRecordsService.Generate(receiptsInRange) != null)
            {
                transactionRecords.AddRange(TransactionRecordsService.Generate(receiptsInRange));
            }

            transactionRecords = TransactionRecordsService.Summarize(transactionRecords, flat.AccountOpenedOn, summarizeTill, flat.OpeningBalance).ToList();

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
                CurrentOutstanding = flat.CurrentOutstanding.FormatNumber("N1"),
                CurrentTotalPenalty = flat.CurrentTotalPenalty.FormatNumber("N1"),
                DateSpecified = flat.DateSpecified.ToString(dateFormat),
                Description = flat.Description,
                Number = flat.Number.ToString(),
                OpeningBalance = flat.OpeningBalance.FormatNumber("N1"),
                OutstandingOnSpecifiedDate = flat.OutstandingOnSpecifiedDate.FormatNumber("N1"),
                PenaltyTillSpecifiedDate = flat.PenaltyTillSpecifiedDate.FormatNumber("N1"),
                OwnedBy = flat.OwnedBy,
                ResidentType = flat.ResidentType.ToString(),
                TenantName = flat.TenantName ?? "-"
            };
        }

        #endregion
    }
}
