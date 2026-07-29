using Microsoft.EntityFrameworkCore;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Infrastructure.Data;


namespace XYZ_shop.Infrastructure.Repositories
{
    public class GameReviewRepository : BaseRepository<GameReviewEntity>, IGameReviewRepository
    {
        public GameReviewRepository(XyzDbContext context) : base(context)
        {
        }

        public bool ExistsForUser(int gameId, int authorId)
        {
            return _dbSet.Any(r => r.GameId == gameId && r.AuthorId == authorId);
        }

        public List<GameReviewEntity> GetByGameId(int gameId)
        {
            return _dbSet
                .Where(r => r.GameId == gameId)
                .ToList();
        }

        public GameReviewEntity? GetWithAuthor(int id)
        {
            return _dbSet
                .Include(r => r.Author)
                .FirstOrDefault(r => r.Id == id);
        }
    }
}
