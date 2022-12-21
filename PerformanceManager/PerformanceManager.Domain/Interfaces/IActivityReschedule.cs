namespace PerformanceManager.Domain.Interfaces
{
    public interface IActivityReschedule
    {
        int Id { get; set; }

        string ReasonForReschedule { get; set; }

        void RescheduleByHours(int hours, IEngineeringActivity activity);

        void RescheduleByDays(int days, IEngineeringActivity engineeringActivity);
    }
}
