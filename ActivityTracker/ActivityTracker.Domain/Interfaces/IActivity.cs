using System;
using System.Collections.Generic;

using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Models;

namespace ActivityTracker.Domain.Interfaces
{
    public interface IActivity
    {

        string Id { get; }

        ActivityMaster ActivityMaster { get; }

        ///ProjectType ProjectType { get; set; }

        IProject Project { get; }

        Discipline Discipline { get; }

        Category Category { get; set; }

        string SubCategory { get; set; }

        string Description { get; set; }

        double AllocatedHours { get; set; }

        HashSet<string> SubCategories { get; }

        HashSet<Category> Categories { get; }

        CompletionStatus CurrentStatus { get; set; }

        HashSet<IResource> AllocatedResources { get; set; }

        HashSet<Change> Changes { get; set; }

        DateTime InitiatedOn { get; set; }

        DateTime ScheduledCompletion { get; }

        bool ValidActivity { get; }

        public void SetCompletionInHours(int totalHoursRequired);

        public void SetCompletionInDays(int totalDaysRequired);
    }
}