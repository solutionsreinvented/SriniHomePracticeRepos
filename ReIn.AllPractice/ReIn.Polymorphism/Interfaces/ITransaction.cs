using ReIn.Polymorphism.Enums;

namespace ReIn.Polymorphism.Interfaces
{
    public interface ITransaction
    {
        TransactionCategory Category { get; set; }
    }
}
