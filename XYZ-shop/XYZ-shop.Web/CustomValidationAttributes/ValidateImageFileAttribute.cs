using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace XYZ_shop.Web.CustomValidationAttributes
{
    public class ValidateImageFileAttribute : ValidationAttribute
    {
        private readonly string[] ALLOWED_EXTENSIONS =
        { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        private readonly long _maxBytes;

        public ValidateImageFileAttribute(long maxBytes = 2 * 1024 * 1024)
        {
            _maxBytes = maxBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not IFormFile file)
            {
                return new ValidationResult("Invalid file.");
            }

            if (file.Length == 0)
            {
                return ValidationResult.Success;
            }

            if (file.Length > _maxBytes)
            {
                return new ValidationResult($"Avatar must be {_maxBytes / (1024 * 1024)} MB or smaller.");
            }

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension) || !ALLOWED_EXTENSIONS.Contains(extension))
            {
                return new ValidationResult("Allowed formats: jpg, jpeg, png, gif, webp.");
            }

            return ValidationResult.Success;
        }
    }
}
