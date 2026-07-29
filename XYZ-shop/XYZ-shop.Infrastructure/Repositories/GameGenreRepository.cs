
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Infrastructure.Data;

namespace XYZ_shop.Infrastructure.Repositories
{
    public class GameGenreRepository : BaseRepository<GameGenreEntity>, IGameGenreRepository
    {
        public GameGenreRepository(XyzDbContext context) : base(context)
        {
        }

        public List<GameGenreEntity> GetByIds(List<int> ids)
        {
            var gameGenres = _dbSet
                .Where(g => ids.Contains(g.Id))
                .ToList();
            return gameGenres;
        }
    }
}
