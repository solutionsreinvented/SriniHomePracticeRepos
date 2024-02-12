using System;
using System.ComponentModel;

using Newtonsoft.Json;

using ReInvented.Shared;
using ReInvented.Shared.Stores;

namespace AllCorePracticeApp
{
    public class Launder : ErrorsEnabledPropertyStore, INotifyDataErrorInfo
    {

        #region Default Constructor

        public Launder()
        {
            Initialize();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Outer diameter of the launder.
        /// </summary>
        [JsonProperty]
        public double OuterDiameter
        {
            get => Get<double>();
            internal set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(InnerDiameter), nameof(Volume), nameof(OuterRadius));
            }
        }
        /// <summary>
        /// Width of the launder.
        /// </summary>
        public double Width
        {
            get => Get<double>();
            //private set
            set
            {
                Set(value);
                RaiseMultiplePropertiesChanged(nameof(InnerDiameter), nameof(Volume), nameof(InnerRadius));
            }
        }
        /// <summary>
        /// Height of the launder wall.
        /// </summary>
        public double Height { get => Get<double>(); set { Set(value); RaisePropertyChanged(nameof(Volume)); } }

        public PlatePropertySelector WallProperty { get => Get<PlatePropertySelector>(); set => Set(value); }

        public PlatePropertySelector FloorProperty { get => Get<PlatePropertySelector>(); set => Set(value); }


        #region Read-only Properties

        /// <summary>
        /// Inner diameter of the launder.
        /// </summary>
        public double InnerDiameter => (OuterDiameter - (2 * Width)).Ceiling(0.001);
        /// <summary>
        /// Volume of the launder of the thickener tank.
        /// </summary>
        public double Volume => Math.PI * (OuterDiameter.Squared() - InnerDiameter.Squared()) / 4 * Height;
        /// <summary>
        /// Outer radius of the launder.
        /// </summary>
        public double OuterRadius => OuterDiameter / 2;
        /// <summary>
        /// Inner radius of the launder.
        /// </summary>
        public double InnerRadius => InnerDiameter / 2;

        #endregion

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Width = 0.0;
            Height = 0.0;
            WallProperty = new PlatePropertySelector();
            FloorProperty = new PlatePropertySelector();
        }

        #endregion

    }
}
