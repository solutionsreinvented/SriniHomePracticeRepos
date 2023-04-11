using System.Collections.Generic;

using ActivityTracker.Domain.Enums;

namespace ActivityTracker.Domain.Interfaces
{
    public interface IResource
    {
        HashSet<IActivity> Activities { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        ResourceRole ResourceRole { get; }

        string FullName { get; }

        int Id { get; set; }
    }
}