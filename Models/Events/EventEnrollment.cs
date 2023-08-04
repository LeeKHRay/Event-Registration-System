using EventRegistrationSystem.Areas.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationSystem.Models.Events
{
    [PrimaryKey(nameof(EventId), nameof(UserId))]
    public class EventEnrollment
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = default!;

        public string UserId { get; set; } = default!;
        public GeneralUser User { get; set; } = default!;

        public DateTime EnrollTime { get; set; } = DateTime.Now;
    }
}
