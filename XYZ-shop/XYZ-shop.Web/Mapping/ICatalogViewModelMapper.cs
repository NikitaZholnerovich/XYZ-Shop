using XYZ_shop.Application.Dtos;
using XYZ_shop.Web.Models;

namespace XYZ_shop.Web.Mapping
{
    public interface ICatalogViewModelMapper
    {
        SteamHomeViewModel ToViewModel(HomeCatalogDto catalog);
        CatalogViewModel ToViewModel(CatalogDto catalog);
        GameDetailsViewModel ToViewModel(GameDetailsDto game);
        CatalogFilterDto ToDto(CatalogFilterViewModel filter);
        SteamGameViewModel ToViewModel(GameDto game);
    }
}
