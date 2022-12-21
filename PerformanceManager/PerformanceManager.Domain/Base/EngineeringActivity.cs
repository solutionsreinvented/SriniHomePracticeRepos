using System;
using System.Collections.Generic;

using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;
using PerformanceManager.Domain.Stores;

namespace PerformanceManager.Domain.Base
{
    public class EngineeringActivity : PropertyStore, IEngineeringActivity
    {
        private const int _workingHoursPerDay = 9;

        public EngineeringActivity()
        {

        }

        protected EngineeringActivity(int activityId)
        {
            InitializeData(activityId);
        }

        public ProjectType ProjectType { get => Get(ProjectType.Order); set => Set(value); }

        public string Description { get => Get<string>(); set => Set(value); }

        public Project Project { get => Get<Project>(); set => Set(value); }

        public EngineeringActivityType ActivityType { get => Get<EngineeringActivityType>(); set => Set(value); }

        public int Id { get => Get<int>(); set => Set(value); }

        public HashSet<IEngineeringResource> AllocatedResources { get => Get<HashSet<IEngineeringResource>>(); set => Set(value); }

        public DateTime InitiatedOn { get => Get<DateTime>(); set => Set(value); }

        public DateTime ScheduledCompletion { get => Get<DateTime>(); set => Set(value); }

        public CompletionStatus CurrentStatus { get => Get(CompletionStatus.OnTrack); set => Set(value); }

        public HashSet<Change> Changes { get => Get<HashSet<Change>>(); set => Set(value); }

        public void SetCompletionInDays(int totalDaysRequired)
        {

        }

        public void SetCompletionInHours(int totalHoursRequired)
        {
            ScheduledCompletion = InitiatedOn.AddDays(totalHoursRequired / _workingHoursPerDay);
        }


        #region Private Helpers

        private void InitializeData(int activityId)
        {
            Id = activityId;
            AllocatedResources = new HashSet<IEngineeringResource>();
            Changes = new HashSet<Change>();
        }

        #endregion

    }
}
