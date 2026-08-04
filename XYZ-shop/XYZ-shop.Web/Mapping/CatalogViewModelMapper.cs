using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XYZ_shop.Web.Mapping
{
    public class CatalogViewModelMapper : ICatalogViewModelMapper
    {
        private readonly IAuthService _authService;

        public CatalogViewModelMapper(IAuthService authService)
        {
            _authService = authService;
        }
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
                IsUserAtLeastModerator = _authService.AtLeastModerator(),
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

        public AddGameViewModel ToViewModel(GameFormOptionsDto form)
        {
            return new AddGameViewModel
            {
                AllGenres = ToGenreSelectList(form.Genres),
                Publishers = ToPublisherSelectList(form.Publishers),
            };
        }

        public EditGameViewModel ToViewModel(EditGameFormDto form)
        {
            return new EditGameViewModel
            {
                Id = form.Game.Id,
                Title = form.Game.Title,
                ImageUrl = form.Game.ImageUrl,
                Description = form.Game.Description,
                Price = form.Game.Price,
                PublisherId = form.Game.PublisherId,
                SelectedGenreIds = form.Game.SelectedGenreIds,
                AllGenres = ToGenreSelectList(form.Options.Genres),
                Publishers = ToPublisherSelectList(form.Options.Publishers),
            };
        }

        public AddGameViewModel FillOptions(AddGameViewModel viewModel, GameFormOptionsDto options)
        {
            viewModel.AllGenres = ToGenreSelectList(options.Genres);
            viewModel.Publishers = ToPublisherSelectList(options.Publishers);
            return viewModel;
        }

        public EditGameViewModel FillOptions(EditGameViewModel viewModel, GameFormOptionsDto options)
        {
            viewModel.AllGenres = ToGenreSelectList(options.Genres);
            viewModel.Publishers = ToPublisherSelectList(options.Publishers);
            return viewModel;
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

        public AddGameDto ToDto(AddGameViewModel game)
        {
            return new AddGameDto
            {
                Title = game.Title,
                ImageUrl = game.ImageUrl,
                Description = game.Description,
                Price = game.Price,
                PublisherId = game.PublisherId,
                SelectedGenreIds = game.SelectedGenreIds,
            };
        }

        public EditGameDto ToDto(EditGameViewModel game)
        {
            return new EditGameDto
            {
                Id = game.Id,
                Title = game.Title,
                ImageUrl = game.ImageUrl,
                Description = game.Description,
                Price = game.Price,
                PublisherId = game.PublisherId ?? 0,
                SelectedGenreIds = game.SelectedGenreIds,
            };
        }

        public GameDetailsViewModel ToViewModel(GameDetailsDto game)
        {
            var userId = _authService.IsAuthenticated() ? _authService.GetUserId() : 0;

            return new GameDetailsViewModel
            {
                IsUserAtLeastModerator = _authService.AtLeastModerator(),
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Price = game.Price,
                AverageRating = game.AverageRating,
                ReviewsCount = game.ReviewsCount,
                PositiveReviewsCount = game.PositiveReviewsCount,
                Genres = game.Genres,
                PublisherName = game.PublisherName,
                PublisherId = game.PublisherId,
                HasUserReviewed = userId > 0 && game.Reviews.Any(r => r.AuthorId == userId),
                Reviews = game.Reviews
                    .Select(r => new GameReviewViewModel
                    {
                        Id = r.Id,
                        GameId = r.GameId,
                        Text = r.Text,
                        Rating = r.Rating,
                        IsRecommended = r.IsRecommended,
                        AuthorId = r.AuthorId,
                        AuthorName = r.AuthorName,
                        AuthorAvatarUrl = r.AuthorAvatarUrl,
                        CreatedAt = r.CreatedAt,
                        ModifiedAt = r.ModifiedAt,
                    })
                    .ToList(),
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

        private List<SelectListItem> ToGenreSelectList(IEnumerable<CatalogGenreDto> genres)
        {
            return genres
                .Select(g => new SelectListItem(g.Name, g.Id.ToString()))
                .ToList();
        }

        private List<SelectListItem> ToPublisherSelectList(IEnumerable<PublisherDto> publishers)
        {
            return publishers
                .Select(p => new SelectListItem(p.Name, p.Id.ToString()))
                .ToList();
        }
    }
}
