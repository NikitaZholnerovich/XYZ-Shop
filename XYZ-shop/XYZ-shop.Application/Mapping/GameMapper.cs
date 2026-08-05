using XYZ_shop.Application.Abstractions.Mapping;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Mapping
{
    public class GameMapper : IGameMapper
    {
        public GameDto ToDto(GameEntity game)
        {
            return new GameDto
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Price = game.Price,
                AverageRating = game.AverageRating,
                ReviewsCount = game.ReviewsCount ?? 0,
                Genres = game.GameGenres.Select(genre => genre.Name).ToList()
            };
        }

        public EditGameDto ToEditDto(GameEntity game)
        {
            return new EditGameDto
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Price = game.Price,
                PublisherId = game.PublisherId,
                SelectedGenreIds = game.GameGenres
                    .Select(genre => genre.Id)
                    .ToList()
            };
        }

        public GameEntity ToEntity(AddGameDto game)
        {
            return new GameEntity
            {
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Price = game.Price,
                PublisherId = game.PublisherId,
                GameGenres = new List<GameGenreEntity>(),
                CreatedAt = DateTime.UtcNow
            };
        }

        public void ApplyEdit(GameEntity game, EditGameDto gameDto)
        {
            game.Title = gameDto.Title;
            game.Description = gameDto.Description;
            game.ImageUrl = gameDto.ImageUrl;
            game.Price = gameDto.Price;
            game.PublisherId = gameDto.PublisherId;
            game.ModifiedAt = DateTime.UtcNow;
        }
    }
}
