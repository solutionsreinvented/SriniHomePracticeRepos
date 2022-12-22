using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Stores;

using System.Collections.Generic;

namespace PerformanceManager.Domain.Models
{
    public class ActivityMaster : PropertyStore
    {
        public ActivityMaster()
        {
            //PreOrders = new HashSet<PreOrder>();
            //Orders = new HashSet<Order>();
        }

        //public HashSet<PreOrder> PreOrders { get => Get<HashSet<PreOrder>>(); set => Set(value); }

        //public HashSet<Order> Orders { get => Get<HashSet<Order>>(); set => Set(value); }

    }
    //public class PreOrder : PropertyStore
    //{
    //    public PreOrder()
    //    {
    //        DesignActivities = new HashSet<DesignActivity>();
    //    }

    //    public string Code { get => Get<string>(); set => Set(value); }

    //    public HashSet<DesignActivity> DesignActivities { get => Get<HashSet<DesignActivity>>(); set => Set(value); }

    //}
    //public class Order : PropertyStore
    //{
    //    public Order()
    //    {
    //        DesignActivities = new HashSet<DesignActivity>();
    //        DetailingActivities = new HashSet<DetailingActivity>();
    //    }

    //    public string Code { get => Get<string>(); set => Set(value); }

    //    public HashSet<DesignActivity> DesignActivities { get => Get<HashSet<DesignActivity>>(); set => Set(value); }

    //    public HashSet<DetailingActivity> DetailingActivities { get => Get<HashSet<DetailingActivity>>(); set => Set(value); }
    //}
}
