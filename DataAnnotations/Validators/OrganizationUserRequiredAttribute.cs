using System.ComponentModel.DataAnnotations;
using static EventRegistrationSystem.Areas.Identity.Pages.Account.RegisterModel;

namespace EventRegistrationSystem.DataAnnotations.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OrganizationUserRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = (InputModel)validationContext.ObjectInstance;

            if (model.Role == "OrganizationUser")
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName! });
                }
            }
            return ValidationResult.Success;
        }
    }
}
