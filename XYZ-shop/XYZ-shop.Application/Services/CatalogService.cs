using XYZ_shop.Application.Abstractions.Mapping;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Domain.HelperModels;

namespace XYZ_shop.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameGenreRepository _gameGenreRepository;
        private readonly IGameMapper _gameMapper;

        public CatalogService(
            IGameRepository gameRepository,
            IGameGenreRepository gameGenreRepository,
            IGameMapper gameMapper)
        {
            _gameRepository = gameRepository;
            _gameGenreRepository = gameGenreRepository;
            _gameMapper = gameMapper;
        }

        public HomeCatalogDto GetGamesForHomePage()
        {
            return new HomeCatalogDto
            {
                Featured = _gameRepository.GetFeaturedForHomePage()
                    .Select(_gameMapper.ToDto)
                    .ToList(),
                SpecialOffers = _gameRepository.GetSpecialOffersForHomePage()
                    .Select(_gameMapper.ToDto)
                    .ToList()
            };
        }

        public CatalogDto GetCatalog(CatalogFilterDto? filter = null)
        {
            filter ??= new CatalogFilterDto();

            var repositoryFilter = new GameFilter
            {
                GenreId = filter.GenreId,
                MaxPrice = filter.MaxPrice,
                SortBy = filter.SortBy,
                SortDirection = filter.SortDirection,
            };

            var games = _gameRepository.GetGames(repositoryFilter, filter.Page, filter.PageSize);
            filter.Page = games.PageIndex;

            return new CatalogDto
            {
                Filter = filter,
                Games = games.Items
                    .Select(_gameMapper.ToDto)
                    .ToList(),
                GameGenres = _gameGenreRepository.GetAll()
                    .OrderBy(g => g.Name)
                    .Select(g => new CatalogGenreDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                    })
                    .ToList(),
                PaginationMetadata = new PaginationMetadataDto
                {
                    CurrentPage = games.PageIndex,
                    PageSize = filter.PageSize,
                    TotalPages = games.TotalPages,
                    TotalCount = games.TotalCount,
                    HasPreviousPage = games.HasPreviousPage,
                    HasNextPage = games.HasNextPage,
                },
            };
        }
    }
}
