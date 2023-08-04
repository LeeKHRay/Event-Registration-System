using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.DataAnnotations.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UserTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            RoleManager<IdentityRole> roleManager = validationContext.GetService<RoleManager<IdentityRole>>()!;
            bool roleExists = roleManager.Roles.Any(role => role.Name == (string?)value);

            if (roleExists)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
