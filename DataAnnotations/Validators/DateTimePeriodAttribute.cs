using EventRegistrationSystem.Services;
using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.DataAnnotations.Validators
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DateTimePeriodAttribute : ValidationAttribute
    {
        public readonly string _enrollStartPropName;
        public readonly string _enrollEndPropName;
        public readonly string _eventStartPropName;
        public readonly string _eventEndPropName;

        public DateTimePeriodAttribute(string enrollStartPropName, string enrollEndPropName, string eventStartPropName, string eventEndPropName)
        {
            _enrollStartPropName = enrollStartPropName;
            _enrollEndPropName = enrollEndPropName;
            _eventStartPropName = eventStartPropName;
            _eventEndPropName = eventEndPropName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Type type = validationContext.ObjectType;
            var model = validationContext.ObjectInstance;

            DateTime[] times =
            {
                (DateTime)type.GetProperty(_enrollStartPropName)!.GetValue(model)!,
                (DateTime)type.GetProperty(_enrollEndPropName)!.GetValue(model)!,
                (DateTime)type.GetProperty(_eventStartPropName)!.GetValue(model)!,
                (DateTime)type.GetProperty(_eventEndPropName)!.GetValue(model)!
            };

            string[] propertyNames =
            {
                _enrollStartPropName,
                _enrollEndPropName,
                _eventStartPropName,
                _eventEndPropName
            };

            var eventService = validationContext.GetService<IEventService>()!;
            var error = eventService.ValidateEventDateTimePeriod(propertyNames, times);
            if (error.HasValue)
            {
                var (errorMessage, memberNames) = error.Value;
                return new ValidationResult(errorMessage, memberNames);
            }

            return ValidationResult.Success;
        }
    }
}
