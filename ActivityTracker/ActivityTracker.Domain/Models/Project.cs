using Newtonsoft.Json;

using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Stores;

using System.Collections.ObjectModel;
using System;

namespace ActivityTracker.Domain.Models
{
    public abstract class Project : PropertyStore, IProject
    {
        #region Events

        public event Action<IProject> ProjectCodeChanged;

        #endregion

        public Project()
        {
            Activities = new ObservableCollection<IActivity>();
        }

        #region Parameterized Constructors

        public Project(string code, string name) : this()
        {
            Code = code;
            Name = name;
            Activities = new ObservableCollection<IActivity>();
        }

        #endregion

        public ProjectType Type { get => Get<ProjectType>(); protected set => Set(value); }

        public string Code { get => Get<string>(); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }

        public string Name { get => Get<string>(); set { Set(value); RaisePropertyChanged(nameof(IsDataValid)); } }

        public ObservableCollection<IActivity> Activities { get; set; }

        public virtual void AddActivity(IActivity activity)
        {
            if (Activities.Contains(activity))
            {
                return;
            }

            Activities.Add(activity);
        }

        public virtual void RemoveActivity(IActivity activity)
        {
            if (!Activities.Contains(activity))
            {
                return;
            }

            _ = Activities.Remove(activity);
        }

        [JsonIgnore]
        public bool IsDataValid => !string.IsNullOrWhiteSpace(Code) && !string.IsNullOrWhiteSpace(Name);
    }
}
