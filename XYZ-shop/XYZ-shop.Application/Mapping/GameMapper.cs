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
                Genres = game.GameGenres.Select(genre => genre.Name).ToList()
            };
        }
    }
}
