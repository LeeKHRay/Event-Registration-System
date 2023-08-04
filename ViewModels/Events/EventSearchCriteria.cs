using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.ViewModels.Events
{
    public class EventSearchCriteria
    {
        [Display(Name = "Event Name")]
        public string? Name { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Sort")]
        public string SortBy { get; set; } = "-CreateTime";
    }
}
