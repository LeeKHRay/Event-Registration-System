using EventRegistrationSystem.Areas.Identity.Models;

namespace EventRegistrationSystem.Models.Events
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
        public string Description { get; set; } = default!;

        public DateTime EnrollStartTime { get; set; }
        public DateTime EnrollEndTime { get; set; }

        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }

        public int Quota { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; }

        public string CreatorId { get; set; } = default!;
        public OrganizationUser Creator { get; set; } = default!;

        public int EventCategoryId { get; set; }
        public EventCategory EventCategory { get; set; } = default!;

        public List<EventImage> EventImages { get; set; } = default!;
        public List<EventEnrollment> EventEnrollments { get; set; } = default!;
    }
}
