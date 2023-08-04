using EventRegistrationSystem.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.ViewModels.Events
{
    public abstract class EventBaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "{0} should have at least {2} character and at most {1} characters")]
        [Display(Name = "Name")]
        public string Name { get; set; } = default!;

        [Required]
        [StringLength(100, ErrorMessage = "{0} should have at most {1} characters")]
        [Display(Name = "Location")]
        public string Location { get; set; } = default!;

        [Required]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "{0} should have at least {2} character and at most {1} characters")]
        [Display(Name = "Introduction")]
        public string Description { get; set; } = default!;

        [Required]
        [Display(Name = "Enrollment start time")]
        [EventTimeDisplayFormat]
        public virtual DateTime EnrollStartTime { get; set; }

        [Required]
        [Display(Name = "Enrollment end time")]
        [EventTimeDisplayFormat]
        public virtual DateTime EnrollEndTime { get; set; }

        [Required]
        [Display(Name = "Event Start time")]
        [EventTimeDisplayFormat]
        public virtual DateTime EventStartTime { get; set; }

        [Required]
        [Display(Name = "Event End time")]
        [EventTimeDisplayFormat]
        public virtual DateTime EventEndTime { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "{0} should be an integer")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} should be between {1} and {2}")]
        [Display(Name = "Quota")]
        public int Quota { get; set; }

        [Display(Name = "Number of applicants")]
        public int ApplicantNum { get; set; }
    }
}
