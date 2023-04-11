using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Stores;

namespace ActivityTracker.Domain.Models
{
    public class Change : PropertyStore
    {
        public Change()
        {

        }

        public int Serial { get => Get<int>(); set => Set(value); }

        public ScheduleChangeReason RescheduleReason { get => Get(ScheduleChangeReason.PriorityChange); set => Set(value); }

        public IActivity AffectedActivity { get => Get<IActivity>(); set => Set(value); }

        public IActivity SupercedingActivity { get => Get<IActivity>(); set => Set(value); }

    }
}
