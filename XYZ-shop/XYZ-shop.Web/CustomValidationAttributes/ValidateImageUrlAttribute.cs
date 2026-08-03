using System.ComponentModel.DataAnnotations;

namespace XYZ_shop.Web.CustomValidationAttributes
{
    public class ValidateImageUrlAttribute : ValidationAttribute
    {
        private readonly string[] ALLOWED_EXTENSIONS =
        { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var url = value as string;

            if (string.IsNullOrWhiteSpace(url))
            {
                return new ValidationResult("URL title cannot be empty or whitespace");
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                return new ValidationResult("Invalid URL format.");
            }

            var lowerUrl = url.ToLower();
            if (!ALLOWED_EXTENSIONS.Any(ext => lowerUrl.EndsWith(ext)))
            {
                return new ValidationResult("URL must point to an image (.jpg, .png, etc.)");
            }

            return ValidationResult.Success;
        }
    }
}
