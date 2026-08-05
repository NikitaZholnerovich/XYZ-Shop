using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using XYZ_shop.Domain.Enums;
using XYZ_shop.Web.CustomValidationAttributes;

namespace XYZ_shop.Web.Models.Profile
{
    public class ProfileViewModel
    {
        public string Login { get; set; } = string.Empty;

        public Language Language { get; set; } = Language.English;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "First name")]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        [StringLength(100)]
        public string? LastName { get; set; }

        [Display(Name = "Phone")]
        [StringLength(30)]
        public string? Mobilephone { get; set; }

        [Display(Name = "Birth date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public string? AvatarUrl { get; set; }

        [Display(Name = "Avatar")]
        [ValidateImageFile]
        public IFormFile? Avatar { get; set; }
    }
}
