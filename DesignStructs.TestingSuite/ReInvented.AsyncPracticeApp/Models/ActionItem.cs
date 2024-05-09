using System;

using ReInvented.AsyncPracticeApp.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.AsyncPracticeApp.Models
{
    public class ActionItem : ValidatablePropertyStore, IActionItem
    {
        #region Default Constructor

        public ActionItem()
        {
            DateTime now = DateTime.Now;
            StartedAt = now;
            TimeElapsed = TimeSpan.MinValue;
        }

        #endregion

        #region Public Properties

        public string Caption { get => Get<string>(); set => Set(value); }

        public string Description { get => Get<string>(); set => Set(value); }

        public DateTime StartedAt { get => Get<DateTime>(); private set => Set(value); }

        public DateTime FinishedAt
        {
            get => Get<DateTime>();
            set
            {
                Set(value);
                if (value > DateTime.MinValue && value > StartedAt)
                {
                    TimeElapsed = (FinishedAt - StartedAt).Duration();
                    TaskCompleted = true;
                }
            }
        }

        public TimeSpan TimeElapsed { get => Get<TimeSpan>(); private set => Set(value); }

        public bool TaskCompleted { get => Get<bool>(); private set => Set(value); }

        #endregion
    }
}
