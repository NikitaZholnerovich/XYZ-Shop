using XYZ_shop.Application.Dtos;
using XYZ_shop.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public CatalogViewModel ToViewModel(CatalogDto catalog)
        {
            return new CatalogViewModel
            {
                Filter = new CatalogFilterViewModel
                {
                    GenreId = catalog.Filter.GenreId,
                    MaxPrice = catalog.Filter.MaxPrice,
                    SortBy = catalog.Filter.SortBy,
                    SortDirection = catalog.Filter.SortDirection,
                    Page = catalog.Filter.Page,
                    PageSize = catalog.Filter.PageSize,
                },
                Games = catalog.Games.Select(ToViewModel).ToList(),
                GameGenres = catalog.GameGenres
                    .Select(g => new SelectListItem(g.Name, g.Id.ToString()))
                    .ToList(),
                PaginationMetadata = new PaginationMetadataViewModel
                {
                    CurrentPage = catalog.PaginationMetadata.CurrentPage,
                    PageSize = catalog.PaginationMetadata.PageSize,
                    TotalPages = catalog.PaginationMetadata.TotalPages,
                    TotalCount = catalog.PaginationMetadata.TotalCount,
                    HasPreviousPage = catalog.PaginationMetadata.HasPreviousPage,
                    HasNextPage = catalog.PaginationMetadata.HasNextPage,
                },
            };
        }

        public CatalogFilterDto ToDto(CatalogFilterViewModel filter)
        {
            return new CatalogFilterDto
            {
                GenreId = filter.GenreId,
                MaxPrice = filter.MaxPrice,
                SortBy = filter.SortBy,
                SortDirection = filter.SortDirection,
                Page = filter.Page,
                PageSize = filter.PageSize,
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
