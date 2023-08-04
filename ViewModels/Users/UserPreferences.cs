using EventRegistrationSystem.DataAnnotations.Validators;

namespace EventRegistrationSystem.ViewModels.Users
{
    public class UserPreferences
    {
        [StringValues("blue", "red", "green", ErrorMessage = "Invalid color")]
        public string Color { get; set; } = default!;
    }
}
