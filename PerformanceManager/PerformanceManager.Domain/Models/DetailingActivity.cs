﻿using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;

namespace PerformanceManager.Domain.Models
{
    public sealed class DetailingActivity : Activity, IActivity
    {
        #region Default Constructor

        public DetailingActivity()
        {
            Discipline = Discipline.Detailing;
            ProjectType = ProjectType.Order;
        }

        #endregion
    }
}
