using XYZ_shop.Application.Dtos;

namespace XYZ_shop.Application.Abstractions.Services
{
    public interface ICatalogService
    {
        HomeCatalogDto GetGamesForHomePage();
        CatalogDto GetCatalog(CatalogFilterDto? filter = null);
    }
}
