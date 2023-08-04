using EventRegistrationSystem.ViewModels.Events;

namespace EventRegistrationSystem.ViewModels.Users
{
    public class OrganizationDetailsViewModel
    {
        public string Name { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string? Website { get; set; }
        public List<EventViewModel> Events { get; set; } = default!;
    }
}
