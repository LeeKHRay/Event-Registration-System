using EventRegistrationSystem.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.ViewModels.Events
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } = default!;

        [Display(Name = "Location")]
        public string Location { get; set; } = default!;

        [Display(Name = "Introduction")]
        public string Description { get; set; } = default!;

        [Display(Name = "Start time")]
        [EventTimeDisplayFormat]
        public DateTime EventStartTime { get; set; }

        [Display(Name = "End time")]
        [EventTimeDisplayFormat]
        public DateTime EventEndTime { get; set; }

        public string EventImageFileName { get; set; } = default!;
    }
}
