using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.DataAnnotations.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StringValuesAttribute : ValidationAttribute
    {
        public readonly string[] _allowedValues;

        public StringValuesAttribute(params string[] allowedValues)
        {
            _allowedValues = allowedValues;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || !_allowedValues.Contains(value.ToString()))
            {
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName! });
            }

            return ValidationResult.Success;
        }
    }
}
