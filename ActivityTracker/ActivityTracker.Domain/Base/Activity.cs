using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Extensions;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Models;
using ActivityTracker.Domain.Services;
using ActivityTracker.Domain.Stores;

using ReInvented.Shared;

namespace ActivityTracker.Domain.Base
{
    public abstract class Activity : PropertyStore, IActivity
    {
        #region Private Constants

        private const int _workingHoursPerDay = 9;

        #endregion

        #region Parameterized Constructor

        public Activity(IProject project)
        {
            Project = project;

            InitializeMandatoryData();
        }

        #endregion

        #region Public Properties

        public Discipline Discipline { get => Get<Discipline>(); set { Set(value); UpdateCategories(); } }

        [JsonProperty]
        public Category Category { get => Get<Category>(); set { Set(value); UpdateSubCategories(); } }

        public string SubCategory { get => Get<string>(); set { Set(value); GenerateActivityId(); } }

        public string Structure { get => Get<string>(); set { Set(value); GenerateActivityId(); } }

        public DateTime InitiatedOn { get => Get<DateTime>(); set { Set(value); UpdateScheduledCompletion(); } }

        public double AllocatedHours { get => Get<double>(); set { Set(value); UpdateScheduledCompletion(); } }

        public HashSet<IResource> AllocatedResources { get => Get<HashSet<IResource>>(); set => Set(value); }

        public CompletionStatus CurrentStatus { get => Get<CompletionStatus>(); set => Set(value); }

        public HashSet<Change> Changes { get => Get<HashSet<Change>>(); set => Set(value); }

        #endregion

        #region Read-only Properties

        [JsonIgnore]
        public ActivityMaster ActivityMaster { get; private set; }

        [JsonProperty]
        public string Id { get => Get<string>(); private set => Set(value); }

        [JsonProperty]
        public IProject Project { get => Get<IProject>(); private set => Set(value); }

        public DateTime ScheduledCompletion { get => Get<DateTime>(); private set => Set(value); }

        [JsonIgnore]
        public HashSet<string> SubCategories { get => Get<HashSet<string>>(); private set => Set(value); }

        [JsonIgnore]
        public HashSet<string> Structures { get => Get<HashSet<string>>(); private set => Set(value); }

        [JsonIgnore]
        public HashSet<Category> Categories { get => Get<HashSet<Category>>(); private set => Set(value); }

        [JsonIgnore]
        public bool ValidActivity => CheckActivityValidity();

        #endregion

        #region Public Methods

        public void SetCompletionInDays(int totalDaysRequired)
        {
            ScheduledCompletion = InitiatedOn.AddDays(totalDaysRequired);
        }

        public void SetCompletionInHours(int totalHoursRequired)
        {
            ScheduledCompletion = InitiatedOn.AddDays((totalHoursRequired / (double)_workingHoursPerDay).Ceiling(1));
        }

        #endregion

        #region Private Methods

        private bool CheckActivityValidity()
        {
            /// TODO: Proper user feedback in terms of validation shall be provided.

            return (Project.Type != ProjectType.Development || Discipline == Discipline.Development) &&
                   (Discipline != Discipline.Detailing || Project.Type == ProjectType.Order) && AllocatedHours > 0;
        }

        private void GenerateActivityId()
        {
            if (SubCategory != null && Project != null)
            {
                string projectBasedCode = $"{Project.Type.GetDescription().Substring(0, 3).ToUpper()}-{Project.Code}";
                string activityCode = $"{(int)Discipline}-{ControlDigitService.Calculate(Category.Name)}-{ControlDigitService.Calculate(SubCategory)}-{ControlDigitService.Calculate(Structure)}";

                Id = $"{projectBasedCode}-{activityCode}";
            }
        }
        private void UpdateScheduledCompletion()
        {
            ScheduledCompletion = InitiatedOn.NextBusinessDay((int)AllocatedHours, HolidaysService.GetAllHolidays());
            RaisePropertyChanged(nameof(ValidActivity));
        }

        private void UpdateSubCategories()
        {
            SubCategories = Categories.FirstOrDefault(c => c.Name == Category.Name).SubCategories;
            SubCategory = SubCategories.FirstOrDefault();

            RaisePropertyChanged(nameof(ValidActivity));

            GenerateActivityId();
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
            #region UnSubscribe and Subscribe to Events
            //Project.ProjectCodeChanged -= OnProjectCodeChanged;
            //Project.ProjectCodeChanged += OnProjectCodeChanged;
            #endregion

            ActivityMaster = ActivityMasterService.ReadFromFile();
            Structures = ActivityMaster.Structures;

            InitiatedOn = DateTime.Today;

            AllocatedResources = new HashSet<IResource>();
            Changes = new HashSet<Change>();
            Structure = Structures?.FirstOrDefault();
        }

        // TODO: Complete the implementation. This method was introduced to update the activity ids when the project code is changed.
        //private void OnProjectCodeChanged(IProject project)
        //{

        //}

        #endregion

    }

    public class ActivityId : PropertyStore
    {
        public ActivityId(IProject project, Discipline discipline, Category category, string subCategory, string structure)
        {

        }

        public string Id => GenerateActivityId();

        public IProject Project { get => Get<IProject>(); set => Set(value); }

        public Discipline Discipline { get => Get<Discipline>(); set => Set(value); }

        public Category Category { get => Get<Category>(); set => Set(value); }

        public string SubCategory { get => Get<string>(); set => Set(value); }

        public string Structure { get => Get<string>(); set => Set(value); }

        private string GenerateActivityId()
        {
            if (SubCategory != null && Project != null)
            {
                string projectBasedCode = $"{Project.Type.GetDescription().Substring(0, 3).ToUpper()}-{Project.Code}";
                string activityCode = $"{(int)Discipline}-{ControlDigitService.Calculate(Category.Name)}-{ControlDigitService.Calculate(SubCategory)}-{ControlDigitService.Calculate(Structure)}";

                return $"{projectBasedCode}-{activityCode}";
            }
            return string.Empty;
        }

    }

}
