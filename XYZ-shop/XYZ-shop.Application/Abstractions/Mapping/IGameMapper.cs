using XYZ_shop.Application.Dtos;
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Abstractions.Mapping
{
    public interface IGameMapper
    {
        GameDto ToDto(GameEntity game);
    }
}
