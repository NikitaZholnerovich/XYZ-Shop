using System.ComponentModel.DataAnnotations;
using XYZ_shop.Application.Abstractions.Repositories;

namespace XYZ_shop.Web.CustomValidationAttributes
{
    /// <summary>
    /// Reflection is a bad choice, when you can do without it.
    /// Used it here just for interest. Better add two attributes (one for add-model, one for edit-model)
    /// </summary>
    public class UniqueGameTitleAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var title = value as string;

            if (string.IsNullOrWhiteSpace(title))
            {
                return new ValidationResult("Game title cannot be empty or whitespace");
            }

            var viewModel = validationContext.ObjectInstance;
            var repository = validationContext.GetRequiredService<IGameRepository>();

            var excludeId = 0;
            var idProperty = viewModel.GetType().GetProperty("Id");

            if (idProperty != null && idProperty.GetValue(viewModel) is int)
            {
                excludeId = (int)idProperty.GetValue(viewModel);
            }

            if (!repository.IsTitleFree(title, excludeId))
            {
                return new ValidationResult("Title is already used");
            }

            return ValidationResult.Success;
        }
    }
}
