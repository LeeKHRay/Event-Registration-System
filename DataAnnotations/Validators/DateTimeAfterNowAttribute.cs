using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.DataAnnotations.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeAfterNowAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime dateTime = (DateTime)value!;
            DateTime now = DateTime.Now;
            now = now.AddSeconds(-now.Second);

            if (dateTime <= now)
            {
                return new ValidationResult("The date and time should not at the past", new[] { validationContext.MemberName! });
            }

            return ValidationResult.Success;
        }
    }
}
