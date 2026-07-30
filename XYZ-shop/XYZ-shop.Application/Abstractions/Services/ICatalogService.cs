using XYZ_shop.Application.Dtos;

namespace XYZ_shop.Application.Abstractions.Services
{
    public interface ICatalogService
    {
        HomeCatalogDto GetGamesForHomePage();
        CatalogDto GetCatalog(CatalogFilterDto? filter = null);
        GameDetailsDto? GetGameDetails(int id);
        GameFormOptionsDto GetGameFormOptions();
        EditGameFormDto? GetEditGameForm(int id);
        void AddGame(AddGameDto game);
        void UpdateGame(EditGameDto game);
    }
}
