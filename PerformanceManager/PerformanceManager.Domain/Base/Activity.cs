using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Extensions;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;
using PerformanceManager.Domain.Services;
using PerformanceManager.Domain.Stores;

using ReInvented.Shared;

namespace PerformanceManager.Domain.Base
{
    public class Activity : PropertyStore, IActivity
    {
        private const int _workingHoursPerDay = 9;

        public Activity()
        {
            InitializeMandatoryData();
        }

        [JsonIgnore]
        public ActivityMaster ActivityMaster { get; private set; }

        public string Id { get => Get<string>(); internal set => Set(value); }

        public Discipline Discipline { get => Get<Discipline>(); set { Set(value); UpdateCategories(); } }

        public Category Category { get => Get<Category>(); set { Set(value); UpdateSubCategories(); } }

        public string SubCategory { get => Get<string>(); set => Set(value); }

        public ProjectType ProjectType { get => Get<ProjectType>(); set => Set(value); }

        public Project Project { get => Get<Project>(); set => Set(value); }

        public DateTime InitiatedOn { get => Get<DateTime>(); set { Set(value); UpdateScheduledCompletion(); } }

        public double AllocatedHours { get => Get<double>(); set { Set(value); UpdateScheduledCompletion(); } }

        public HashSet<IResource> AllocatedResources { get => Get<HashSet<IResource>>(); set => Set(value); }


        public DateTime ScheduledCompletion { get => Get<DateTime>(); private set => Set(value); }

        public CompletionStatus CurrentStatus { get => Get<CompletionStatus>(); set => Set(value); }

        public HashSet<Change> Changes { get => Get<HashSet<Change>>(); set => Set(value); }

        [JsonIgnore]
        public HashSet<string> SubCategories { get => Get<HashSet<string>>(); protected set => Set(value); }

        [JsonIgnore]
        public HashSet<Category> Categories { get => Get<HashSet<Category>>(); protected set => Set(value); }



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

        #region Private Methods

        private void UpdateScheduledCompletion()
        {
            ScheduledCompletion = InitiatedOn.NextBusinessDay((int)AllocatedHours, HolidaysService.GetAllHolidays());
        }

        private void UpdateSubCategories()
        {
            SubCategories = Categories.FirstOrDefault(c => c.Name == Category.Name).SubCategories;
            SubCategory = SubCategories.FirstOrDefault();
        }

        private void UpdateCategories()
        {
            Categories = ActivityMaster.Domains.FirstOrDefault(d => d.Descipline == Discipline).Categories;
            Category = Categories.FirstOrDefault();
        }

        #endregion

        #region Private Helpers

        protected void InitializeMandatoryData()
        {
            AllocatedResources = new HashSet<IResource>();
            Changes = new HashSet<Change>();

            ActivityMaster = ActivityMasterService.ReadFromFile();
            InitiatedOn = DateTime.Today;
        }

        #endregion

    }
}
