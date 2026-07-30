using XYZ_shop.Application.Abstractions.Mapping;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Domain.HelperModels;

namespace XYZ_shop.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameGenreRepository _gameGenreRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IGameMapper _gameMapper;

        public CatalogService(
            IGameRepository gameRepository,
            IGameGenreRepository gameGenreRepository,
            IPublisherRepository publisherRepository,
            IGameMapper gameMapper)
        {
            _gameRepository = gameRepository;
            _gameGenreRepository = gameGenreRepository;
            _publisherRepository = publisherRepository;
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

        public GameDetailsDto? GetGameDetails(int id)
        {
            var game = _gameRepository.GetGameDetails(id);
            if (game == null)
            {
                return null;
            }

            var reviews = game.GameReviews ?? new();

            return new GameDetailsDto
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Price = game.Price,
                AverageRating = reviews.Any()
                    ? Math.Round(reviews.Average(r => r.Rating), 1)
                    : null,
                ReviewsCount = reviews.Count,
                PositiveReviewsCount = reviews.Count(r => r.Rating >= 7),
                Genres = game.GameGenres?
                    .Select(g => g.Name)
                    .ToList() ?? new(),
                PublisherName = game.Publisher?.Name,
                PublisherId = game.PublisherId,
                Reviews = reviews
                    .OrderByDescending(r => r.CreatedAt)
                    .Select(r => new GameReviewDto
                    {
                        Id = r.Id,
                        GameId = r.GameId,
                        Text = r.Text,
                        Rating = r.Rating,
                        IsRecommended = r.Rating >= 7,
                        AuthorId = r.AuthorId,
                        AuthorName = r.Author?.Login ?? "Unknown",
                        CreatedAt = r.CreatedAt,
                        ModifiedAt = r.ModifiedAt,
                    })
                    .ToList(),
            };
        }

        private List<PublisherDto> GetPublishers()
        {
            return _publisherRepository.GetAll()
                .OrderBy(p => p.Name)
                .Select(p => new PublisherDto
                {
                    Id = p.Id,
                    Name = p.Name,
                })
                .ToList();
        }

        private List<CatalogGenreDto> GetGameGenres()
        {
            return _gameGenreRepository.GetAll()
                .OrderBy(g => g.Name)
                .Select(g => new CatalogGenreDto
                {
                    Id = g.Id,
                    Name = g.Name,
                })
                .ToList();
        }

        public GameFormOptionsDto GetGameFormOptions()
        {
            return new GameFormOptionsDto
            {
                Genres = GetGameGenres(),
                Publishers = GetPublishers(),
            };
        }

        public EditGameFormDto? GetEditGameForm(int id)
        {
            var game = _gameRepository.GetGameDetails(id);
            if (game == null)
            {
                return null;
            }

            return new EditGameFormDto
            {
                Game = _gameMapper.ToEditDto(game),
                Options = GetGameFormOptions(),
            };
        }

        public void AddGame(AddGameDto game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game), "Game data cannot be null");
            }

            var gameEntity = _gameMapper.ToEntity(game);

            if (game.SelectedGenreIds.Any())
            {
                var genres = _gameGenreRepository.GetByIds(game.SelectedGenreIds);
                foreach (var genre in genres)
                {
                    gameEntity.GameGenres.Add(genre);
                }
            }

            _gameRepository.Add(gameEntity);
        }

        public void UpdateGame(EditGameDto gameDto)
        {
            var game = _gameRepository.GetGameDetails(gameDto.Id);

            if (game == null)
            {
                throw new ArgumentException("Game not found");
            }

            _gameMapper.ApplyEdit(game, gameDto);

            game.GameGenres.Clear();

            if (gameDto.SelectedGenreIds.Any())
            {
                var genres = _gameGenreRepository.GetByIds(gameDto.SelectedGenreIds);
                foreach (var genre in genres)
                {
                    game.GameGenres.Add(genre);
                }
            }

            _gameRepository.Update(game);
        }
    }
}
