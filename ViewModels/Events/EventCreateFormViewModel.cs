using EventRegistrationSystem.DataAnnotations.Validators;

namespace EventRegistrationSystem.ViewModels.Events
{
    [DateTimePeriod(nameof(EnrollStartTime), nameof(EnrollEndTime), nameof(EventStartTime), nameof(EventEndTime))]
    public class EventCreateFormViewModel : EventFormViewModel
    {
        [DateTimeAfterNow]
        public override DateTime EnrollStartTime { get; set; }

        [DateTimeAfterNow]
        public override DateTime EnrollEndTime { get; set; }

        [DateTimeAfterNow]
        public override DateTime EventStartTime { get; set; }

        [DateTimeAfterNow]
        public override DateTime EventEndTime { get; set; }
    }
}
