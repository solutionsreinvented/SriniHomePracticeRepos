﻿using System.Collections.Generic;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class PCDLoads
    {
        public PCDLoads()
        {

        }

        public string PCD { get; set; }

        public SupportsInformation SupportsInformation { get; set; }

        public HashSet<LoadCaseForces> SupportLoadsSummary { get; set; }

        public HashSet<SupportLoads> SupportLoadsCollection { get; set; }
    }
}