using System;

using static ActivityTracker.Domain.Enums.PerformanceRating;

namespace ActivityTracker.Domain.Services
{
    public class PerformanceCalculationService
    {
        public PerformanceCalculationService(int hoursAllocated, int hoursConsumed)
        {
            HoursAllocated = hoursAllocated;
            HoursConsumed = hoursConsumed;
        }

        public double HoursAllocated { get; private set; }

        public double HoursConsumed { get; private set; }

        public double HoursSaved => HoursAllocated - HoursConsumed > 0.0 ? HoursAllocated - HoursConsumed : 0.0;

        public double RatingFactor => CalculatedRatingFactor();

        private double CalculatedRatingFactor()
        {
            double hoursToBeSavedForOutstanding = (Outstanding - Satisfactory) * HoursAllocated / (double)Outstanding;
            double effectiveHours = HoursConsumed <= HoursAllocated ? HoursAllocated : HoursConsumed;

            double baseRating = (double)Satisfactory * (HoursAllocated / effectiveHours);
            double beneficialRating = (Outstanding - Satisfactory) * HoursSaved / hoursToBeSavedForOutstanding;

            double ratingFactor = baseRating + beneficialRating;

            return Math.Round(ratingFactor, 3);
        }
    }
}
