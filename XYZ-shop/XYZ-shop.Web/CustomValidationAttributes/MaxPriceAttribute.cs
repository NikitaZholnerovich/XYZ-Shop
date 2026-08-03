using System.ComponentModel.DataAnnotations;

namespace XYZ_shop.Web.CustomValidationAttributes
{
    public class MaxPriceAttribute : ValidationAttribute
    {
        private const decimal MAX_PRICE = 200.00M;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not decimal)
            {
                return new ValidationResult("Price must be a number");
            }

            var price = (decimal)value;

            if (price < 0)
            {
                return new ValidationResult("Price cannot be negative");
            }

            if (price > MAX_PRICE)
            {
                return new ValidationResult($"Price cannot be more than {MAX_PRICE}$");
            }

            return ValidationResult.Success;
        }
    }
}
