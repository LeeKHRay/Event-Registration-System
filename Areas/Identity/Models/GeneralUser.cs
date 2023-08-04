using EventRegistrationSystem.Models.Events;

namespace EventRegistrationSystem.Areas.Identity.Models;

public class GeneralUser : ApplicationUser
{
    public ICollection<EventEnrollment> EventEnrollments { get; set; } = default!;
}
