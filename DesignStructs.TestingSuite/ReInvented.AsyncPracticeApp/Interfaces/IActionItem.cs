using System;

namespace ReInvented.AsyncPracticeApp.Interfaces
{
    public interface IActionItem
    {
        string Caption { get; set; }

        string Description { get; set; }

        DateTime StartedAt { get; }

        DateTime FinishedAt { get; set; }

        TimeSpan TimeElapsed { get; }

        bool TaskCompleted { get; }
    }
}
