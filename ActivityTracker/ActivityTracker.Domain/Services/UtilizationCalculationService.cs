using ActivityTracker.Domain.Interfaces;

namespace ActivityTracker.Domain.Services
{
    public class UtilizationCalculationService
    {

        public UtilizationCalculationService(IYear year)
        {
            AssessmentYear = year;
        }

        public IYear AssessmentYear { get; private set; }

    }
}
