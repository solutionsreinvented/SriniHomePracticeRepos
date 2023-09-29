﻿using ReInvented.Shared.Stores;

namespace ReInvented.FluentValidationExercise.Models
{
    public class ThickenerInput : ErrorsEnabledPropertyStore
    {
        public ThickenerInput()
        {

        }

        public Shell Shell { get => Get<Shell>(); set => Set(value); }

        public Feedwell FeedWell { get => Get<Feedwell>(); set => Set(value); }
    }
}