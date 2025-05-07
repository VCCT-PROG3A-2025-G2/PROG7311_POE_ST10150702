using System.ComponentModel.DataAnnotations;

namespace PROG7311_POE_ST10150702.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object _targetValue;

        public RequiredIfAttribute(string propertyName, object targetValue)
        {
            _propertyName = propertyName;
            _targetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var property = instance.GetType().GetProperty(_propertyName);

            if (property == null)
                return new ValidationResult($"Unknown property: {_propertyName}");

            var propertyValue = property.GetValue(instance);

            if (propertyValue?.ToString() == _targetValue.ToString() && value == null)
            {
                return new ValidationResult(ErrorMessage ?? $"{context.DisplayName} is required.");
            }

            return ValidationResult.Success;
        }
    }
}