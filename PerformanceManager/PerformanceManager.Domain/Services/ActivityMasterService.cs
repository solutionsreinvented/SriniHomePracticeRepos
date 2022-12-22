using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Models;

using System;

namespace PerformanceManager.Domain.Services
{
    public static class ActivityMasterService
    {
        public static ActivityMaster Create()
        {
            ActivityMaster activityMaster = new();

            //var preorder = new PreOrder() { Code = "GS2212129" };

            //preorder.DesignActivities.Add(new DesignActivity(1) { ProjectType = ProjectType.PreOrder, InitiatedOn = DateTime.Today });
            //preorder.DesignActivities.Add(new DesignActivity(2) { ProjectType = ProjectType.PreOrder, InitiatedOn = DateTime.Today });
            //preorder.DesignActivities.Add(new DesignActivity(3) { ProjectType = ProjectType.PreOrder, InitiatedOn = DateTime.Today });
            //preorder.DesignActivities.Add(new DesignActivity(4) { ProjectType = ProjectType.PreOrder, InitiatedOn = DateTime.Today });


            //activityMaster.PreOrders.Add(preorder);

            return activityMaster;
        }
    }
}
