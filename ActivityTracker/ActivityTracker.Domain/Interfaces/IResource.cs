using System.Collections.Generic;

using ActivityTracker.Domain.Enums;

namespace ActivityTracker.Domain.Interfaces
{
    public interface IResource
    {
        int Id { get; set; }

        string FullName { get; }

        string FirstName { get; set; }

        string LastName { get; set; }

        HashSet<IActivity> Activities { get; set; }

        ResourceRole ResourceRole { get; }
    }
}