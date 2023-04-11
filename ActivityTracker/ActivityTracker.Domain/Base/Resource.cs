using System.Collections.Generic;

using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Stores;

namespace ActivityTracker.Domain.Base
{
    public class Resource : PropertyStore, IResource
    {
        #region Default Constructor

        public Resource()
        {

        }

        #endregion

        protected Resource(int employeeId)
        {
            InitializeData(employeeId);
        }

        public int Id { get => Get<int>(); set => Set(value); }

        public HashSet<IActivity> Activities { get => Get<HashSet<IActivity>>(); set => Set(value); }

        public ResourceRole ResourceRole { get => Get<ResourceRole>(); set => Set(value); }

        public string FirstName
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(FullName));
            }
        }

        public string LastName
        {
            get => Get<string>();
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(FullName));
            }
        }

        public string FullName => $"{FirstName} {LastName}";


        #region Private Helpers

        private void InitializeData(int employeeId)
        {
            Id = employeeId;
            Activities = new HashSet<IActivity>();
        }

        #endregion

    }
}
