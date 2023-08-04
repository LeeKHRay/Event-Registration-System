using EventRegistrationSystem.ViewModels.Users;
using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.ViewModels.Events
{
    public class EventDetailsViewModel : EventBaseViewModel
    {
        [Display(Name = "Category")]
        public string Category { get; set; } = default!;

        [Display(Name = "Organization")]
        public OrganizationViewModel Organization { get; set; } = default!;

        public List<string> EventImageFileNames { get; set; } = default!;
    }
}
