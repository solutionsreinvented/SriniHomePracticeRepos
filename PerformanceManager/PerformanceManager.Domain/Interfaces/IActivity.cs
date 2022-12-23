using System;
using System.Collections.Generic;

using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Models;

namespace PerformanceManager.Domain.Interfaces
{
    public interface IActivity
    {

        string Id { get; }

        ActivityMaster ActivityMaster { get; }

        ProjectType ProjectType { get; set; }

        Project Project { get; set; }

        Discipline Discipline { get; }

        Category Category { get; set; }

        string SubCategory { get; set; }

        double AllocatedHours { get; set; }

        HashSet<string> SubCategories { get; }

        HashSet<Category> Categories { get; }

        CompletionStatus CurrentStatus { get; set; }

        HashSet<IResource> AllocatedResources { get; set; }

        HashSet<Change> Changes { get; set; }

        DateTime InitiatedOn { get; set; }

        DateTime ScheduledCompletion { get; }

        public void SetCompletionInHours(int totalHoursRequired);

        public void SetCompletionInDays(int totalDaysRequired);
    }
}