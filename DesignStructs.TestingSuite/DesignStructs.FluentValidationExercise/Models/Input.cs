using ReInvented.Shared.Stores;

namespace ReInvented.FluentValidationExercise.Models
{
    public class Input : ValidatablePropertyStore
    {
        public Input()
        {
            Shell = new Shell(this);
            FeedWell = new FeedWell(this);
        }

        public Shell Shell { get => Get<Shell>(); set => Set(value); }

        public FeedWell FeedWell { get => Get<FeedWell>(); set => Set(value); }

    }
}
