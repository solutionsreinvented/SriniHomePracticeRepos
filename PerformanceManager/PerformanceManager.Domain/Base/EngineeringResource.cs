using System.Collections.Generic;

using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Stores;

namespace PerformanceManager.Domain.Base
{
    public class EngineeringResource : PropertyStore, IEngineeringResource
    {
        #region Default Constructor

        public EngineeringResource()
        {

        }

        #endregion

        protected EngineeringResource(int employeeId)
        {
            InitializeData(employeeId);
        }

        public List<IEngineeringActivity> Activities { get => Get<List<IEngineeringActivity>>(); set => Set(value); }

        public ResourceRole ResourceRole { get => Get<ResourceRole>(); set => Set(value); }

        public int Id { get => Get<int>(); set => Set(value); }

        public string FirstName
        {
            get => Get<string>();
            set
            {
                Set(value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string LastName
        {
            get => Get<string>();
            set
            {
                Set(value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string FullName => $"{FirstName} {LastName}";


        #region Private Helpers

        private void InitializeData(int employeeId)
        {
            Id = employeeId;
            Activities = new List<IEngineeringActivity>();
        }

        #endregion

    }
}
