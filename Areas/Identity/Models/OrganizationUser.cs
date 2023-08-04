using EventRegistrationSystem.Models.Events;

namespace EventRegistrationSystem.Areas.Identity.Models;

public class OrganizationUser : ApplicationUser
{
    public string OrganizationName { get; set; } = default!;
    public string? Website { get; set; }

    public ICollection<Event> Events { get; set; } = default!;
}
