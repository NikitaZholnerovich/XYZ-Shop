using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using XYZ_shop.Web.CustomValidationAttributes;

namespace XYZ_shop.Web.Models
{
    public class EditGameViewModel
    {
        public int Id { get; set; }

        [Required]
        [UniqueGameTitle]
        [NoSpecialCharacters]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [NoSpecialCharacters]
        public string Description { get; set; } = string.Empty;

        [Required]
        [ValidateImageUrl]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [MaxPrice]
        public decimal Price { get; set; }
        public int? PublisherId { get; set; }
        public List<int> SelectedGenreIds { get; set; } = new();

        public List<SelectListItem> AllGenres { get; set; } = new();
        public List<SelectListItem> Publishers { get; set; } = new();
    }
}
