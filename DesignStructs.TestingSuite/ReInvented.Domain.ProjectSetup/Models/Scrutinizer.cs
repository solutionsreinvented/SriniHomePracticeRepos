using System;

using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.ProjectSetup.Models
{
    /// <summary>
    /// Tracks the review and approval process of an engineering task. Scrutinizer is a person who creates, reviews and/or approves a task.
    /// This keeps track of Scrutinizer's fullname, initials and the date of scrutiny.
    /// </summary>
    public class Scrutinizer : ErrorsEnabledPropertyStore, IScrutinizer
    {
        #region Default Constructor

        public Scrutinizer()
        {
            DateOfScrutiny = DateTime.Today;
        }

        #endregion

        #region Public Properties

        public string FullName { get => Get<string>(); set => Set(value); }

        public string ShortName { get => Get<string>(); set => Set(value); }

        public DateTime DateOfScrutiny { get => Get<DateTime>(); set => Set(value); }

        #endregion
    }
}
