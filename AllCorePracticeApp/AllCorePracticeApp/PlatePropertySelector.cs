using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

using ReInvented.Shared.Stores;

namespace AllCorePracticeApp
{
    public class PlatePropertySelector : ErrorsEnabledPropertyStore
    {
        #region Default Constructor

        public PlatePropertySelector()
        {

        }

        #endregion

        public double Thickness { get => Get<double>(); set => Set(value);}
    }
}
