using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Abstractions.Repositories
{
    public interface IGameReviewRepository : IBaseRepository<GameReviewEntity>
    {
        bool ExistsForUser(int gameId, int authorId);
        List<GameReviewEntity> GetByGameId(int gameId);
        GameReviewEntity? GetWithAuthor(int id);
    }
}
