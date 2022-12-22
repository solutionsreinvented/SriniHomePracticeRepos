using System;
using System.Collections.Generic;

using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;
using PerformanceManager.Domain.Stores;

using ReInvented.Shared;

namespace PerformanceManager.Domain.Base
{
    public class Activity : PropertyStore, IActivity
    {
        private const int _workingHoursPerDay = 9;

        public Activity()
        {

        }

        protected Activity(int id)
        {
            Initialize(id);
        }

        public int Id { get => Get<int>(); protected set => Set(value); }

        public string Description { get => Get<string>(); set => Set(value); }

        public ProjectType ProjectType { get => Get<ProjectType>(); set => Set(value); }

        public Project Project { get => Get<Project>(); set => Set(value); }

        public Enums.Domain ActivityType { get => Get<Enums.Domain>(); protected set => Set(value); }

        public HashSet<IResource> AllocatedResources { get => Get<HashSet<IResource>>(); set => Set(value); }

        public DateTime InitiatedOn { get => Get<DateTime>(); set => Set(value); }

        public DateTime ScheduledCompletion { get => Get<DateTime>(); private set => Set(value); }

        public CompletionStatus CurrentStatus { get => Get<CompletionStatus>(); set => Set(value); }

        public HashSet<Change> Changes { get => Get<HashSet<Change>>(); set => Set(value); }

        #region Public Methods

        public void SetCompletionInDays(int totalDaysRequired)
        {
            ScheduledCompletion = InitiatedOn.AddDays(totalDaysRequired);
        }

        public void SetCompletionInHours(int totalHoursRequired)
        {
            ScheduledCompletion = InitiatedOn.AddDays((totalHoursRequired / (double)_workingHoursPerDay).CeilingTo(1));
        }

        #endregion


        #region Private Helpers

        private void Initialize(int id)
        {
            Id = id;

            AllocatedResources = new HashSet<IResource>();
            Changes = new HashSet<Change>();
        }

        #endregion

    }
}
