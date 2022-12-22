using System;
using System.Collections.Generic;

using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Models;

namespace PerformanceManager.Domain.Interfaces
{
    public interface IActivity
    {

        int Id { get; }

        string Description { get; set; }

        ProjectType ProjectType { get; set; }

        Project Project { get; set; }

        ActivityType ActivityType { get; }

        CompletionStatus CurrentStatus { get; set; }

        HashSet<IResource> AllocatedResources { get; set; }

        HashSet<Change> Changes { get; set; }

        DateTime InitiatedOn { get; set; }

        DateTime ScheduledCompletion { get; }

        public void SetCompletionInHours(int totalHoursRequired);

        public void SetCompletionInDays(int totalDaysRequired);
    }
}