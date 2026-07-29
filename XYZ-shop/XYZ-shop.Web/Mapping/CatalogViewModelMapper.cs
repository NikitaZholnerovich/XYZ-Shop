using XYZ_shop.Application.Dtos;
using XYZ_shop.Web.Models;

namespace XYZ_shop.Web.Mapping
{
    public class CatalogViewModelMapper : ICatalogViewModelMapper
    {
        public SteamHomeViewModel ToViewModel(HomeCatalogDto catalog)
        {
            return new SteamHomeViewModel
            {
                Featured = catalog.Featured.Select(ToViewModel).ToList(),
                SpecialOffers = catalog.SpecialOffers.Select(ToViewModel).ToList()
            };
        }

        public SteamGameViewModel ToViewModel(GameDto game)
        {
            return new SteamGameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Price = game.Price,
                Genres = game.Genres
            };
        }
    }
}
