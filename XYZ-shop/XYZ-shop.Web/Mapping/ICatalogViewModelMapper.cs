using XYZ_shop.Application.Dtos;
using XYZ_shop.Web.Models;

namespace XYZ_shop.Web.Mapping
{
    public interface ICatalogViewModelMapper
    {
        SteamHomeViewModel ToViewModel(HomeCatalogDto catalog);
        CatalogViewModel ToViewModel(CatalogDto catalog);
        GameDetailsViewModel ToViewModel(GameDetailsDto game);
        AddGameViewModel ToAddGameViewModel(List<CatalogGenreDto> genres, List<PublisherDto> publishers);
        CatalogFilterDto ToDto(CatalogFilterViewModel filter);
        AddGameDto ToDto(AddGameViewModel game);
        SteamGameViewModel ToViewModel(GameDto game);
    }
}
