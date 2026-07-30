using XYZ_shop.Application.Dtos;
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Abstractions.Mapping
{
    public interface IGameMapper
    {
        GameDto ToDto(GameEntity game);
        EditGameDto ToEditDto(GameEntity game);
        GameEntity ToEntity(AddGameDto game);
        void ApplyEdit(GameEntity game, EditGameDto gameDto);
    }
}
