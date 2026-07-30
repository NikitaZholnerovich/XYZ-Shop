using XYZ_shop.Application.Dtos;
using XYZ_shop.Web.Models;

namespace XYZ_shop.Web.Mapping
{
    public interface ICatalogViewModelMapper
    {
        SteamHomeViewModel ToViewModel(HomeCatalogDto catalog);
        CatalogViewModel ToViewModel(CatalogDto catalog);
        GameDetailsViewModel ToViewModel(GameDetailsDto game);
        AddGameViewModel ToViewModel(GameFormOptionsDto form);
        EditGameViewModel ToViewModel(EditGameFormDto form);
        AddGameViewModel FillOptions(AddGameViewModel viewModel, GameFormOptionsDto options);
        EditGameViewModel FillOptions(EditGameViewModel viewModel, GameFormOptionsDto options);
        CatalogFilterDto ToDto(CatalogFilterViewModel filter);
        AddGameDto ToDto(AddGameViewModel game);
        EditGameDto ToDto(EditGameViewModel game);
        SteamGameViewModel ToViewModel(GameDto game);
    }
}
