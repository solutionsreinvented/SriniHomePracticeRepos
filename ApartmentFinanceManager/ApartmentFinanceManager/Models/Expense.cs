using System;

using ApartmentFinanceManager.Enums;

using ReInvented.Shared.Store;

namespace ApartmentFinanceManager.Models
{
    public class Expense : PropertyStore
    {
        public Expense()
        {

        }

        public Expense(ExpenseCategory expenseCategory, decimal amount)
            : this(expenseCategory, amount, DateOnly.FromDateTime(DateTime.Today))
        {

        }

        public Expense(ExpenseCategory expenseCategory, decimal amount, DateOnly occuredOn)
        {
            ExpenseCategory = expenseCategory;
            Amount = amount;
            OccuredOn = occuredOn;
        }
        /// <summary>
        /// Date(only) on which the expenditure is recorded.
        /// </summary>
        public DateOnly OccuredOn { get => Get<DateOnly>(); set => Set(value); }
        /// <summary>
        /// Category <see cref="Enums.ExpenseCategory"/> of the expense.
        /// </summary>
        public ExpenseCategory ExpenseCategory { get => Get(ExpenseCategory.Maintenance); set => Set(value); }
        /// <summary>
        /// Amount spent.
        /// </summary>
        public decimal Amount { get => Get<decimal>(); set => Set(value); }
    }
}