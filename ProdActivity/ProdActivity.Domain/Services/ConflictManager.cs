using System;
using System.Collections.Generic;
using System.Linq;

using ProdActivity.Domain.Interfaces;

namespace ProdActivity.Domain.Services
{
    public class ConflictManager
    {
        public bool HasConflict(IResource resource, DateTime startDate, DateTime endDate, IEnumerable<IActivity> allActivities, out IActivity conflictingActivity)
        {
            conflictingActivity = null;

            if (resource == null || allActivities == null)
                return false;

            foreach (var activity in allActivities)
            {
                // Skip if the resource is not allocated to this activity
                if (!activity.AllocatedResources.Any(r => r.Id == resource.Id))
                    continue;

                // Check for overlapping dates
                if (startDate.Date <= activity.ScheduledCompletion.Date && endDate.Date >= activity.InitiatedOn.Date)
                {
                    conflictingActivity = activity;
                    return true; // Conflict found
                }
            }

            return false;
        }
    }
}
