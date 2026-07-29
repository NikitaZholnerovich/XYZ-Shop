
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Abstractions.Repositories
{
    public interface IGameGenreRepository : IBaseRepository<GameGenreEntity>
    {
        List<GameGenreEntity> GetByIds(List<int> ids);
    }
}
