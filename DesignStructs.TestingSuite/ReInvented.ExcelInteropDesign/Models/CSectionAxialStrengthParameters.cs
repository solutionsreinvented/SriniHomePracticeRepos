﻿using ReInvented.ExcelInteropDesign.Base;
using ReInvented.ExcelInteropDesign.Enums;
using ReInvented.ExcelInteropDesign.Interfaces;

namespace ReInvented.ExcelInteropDesign.Models
{
    public sealed class CSectionAxialStrengthParameters : AxialStrengthParameters, IAxialStrengthParameters, IGussetParameters
    {
        #region Default Constructor

        public CSectionAxialStrengthParameters()
        {
            GussetConnectedTo = GussetToCSectionConnectionElement.Web;
        }

        #endregion

        /// <summary>
        /// Indicates where the gusset plate(s) is(are) connected to C section.
        /// </summary>
        public GussetToCSectionConnectionElement GussetConnectedTo { get => Get<GussetToCSectionConnectionElement>(); set => Set(value); }
        /// <summary>
        /// Net sectional area for the tensile strength calculations.
        /// </summary>
        public double Anet { get => Get<double>(); set => Set(value); }
        /// <summary>
        /// Torsional unbraced length of the member
        /// </summary>
        public double Ltub => Lus;

        public double Tg { get => Get<double>(); set => Set(value); }

        public double Lcon { get => Get<double>(); set => Set(value); }

    }
}
