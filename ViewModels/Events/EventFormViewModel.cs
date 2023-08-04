using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.ViewModels.Events
{
    public abstract class EventFormViewModel : EventBaseViewModel
    {
        [Required]
        [Display(Name = "Category")]
        public int EventCategoryId { get; set; }

        public List<IFormFile>? FormImages { get; set; }
        public List<int>? UploadedImageIds { get; set; }
    }
}
