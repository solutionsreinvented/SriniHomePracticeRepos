using ReInvented.Shared.Stores;

namespace Axiom.Financials.Models
{
    public class FinancialYear : ValidatablePropertyStore
    {
        public string Id { get => Get<string>(); set => Set(value); }
    }
}
