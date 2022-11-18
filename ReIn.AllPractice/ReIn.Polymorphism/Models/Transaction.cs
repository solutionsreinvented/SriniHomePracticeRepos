using System;

using ReIn.Polymorphism.Enums;
using ReIn.Polymorphism.Interfaces;

namespace ReIn.Polymorphism.Models
{
    public abstract class Transaction : ITransaction
    {
        public TransactionCategory Category { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
