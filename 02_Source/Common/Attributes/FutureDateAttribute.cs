using System.ComponentModel.DataAnnotations;

namespace Common.Attributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dueDate = (DateTime)value;
            if (dueDate < DateTime.Today)
            {
                return new ValidationResult("Due date must be a future date.");
            }
            return ValidationResult.Success;
        }
    }
}
