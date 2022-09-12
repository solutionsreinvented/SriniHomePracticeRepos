using System;

using ApartmentFinanceManager.Enums;

using ReInvented.Shared.Stores;

namespace ApartmentFinanceManager.Models
{
    public class Expense : PropertyStore
    {
        public Expense()
        {

        }

        public Expense(TransactionCategory expenseCategory, decimal amount)
            : this(expenseCategory, amount, DateTime.Today)
        {

        }

        public Expense(TransactionCategory expenseCategory, decimal amount, DateTime occuredOn)
        {
            ExpenseCategory = expenseCategory;
            Amount = amount;
            OccuredOn = occuredOn;
        }
        /// <summary>
        /// Date on which the expenditure is recorded.
        /// </summary>
        public DateTime OccuredOn { get => Get(DateTime.Today); set => Set(value); }
        /// <summary>
        /// Category <see cref="TransactionCategory"/> of the expense.
        /// </summary>
        public TransactionCategory ExpenseCategory { get => Get(TransactionCategory.Maintenance); set => Set(value); }
        /// <summary>
        /// Amount spent.
        /// </summary>
        public decimal Amount { get => Get<decimal>(); set => Set(value); }
    }
}