using Microsoft.AspNetCore.Mvc.Rendering;

namespace XYZ_shop.Web.Models
{
    public class CatalogViewModel
    {
        public bool IsUserAtLeastModerator { get; set; }
        public CatalogFilterViewModel Filter { get; set; } = new();
        public List<SteamGameViewModel> Games { get; set; } = new();
        public List<SelectListItem> GameGenres { get; set; } = new();
        public List<SelectListItem> Publishers { get; set; } = new();

        public PaginationMetadataViewModel PaginationMetadata { get; set; } = new();
    }
}
