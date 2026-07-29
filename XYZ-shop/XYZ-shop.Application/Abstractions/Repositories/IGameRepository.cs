
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Abstractions.Repositories
{
    public interface IGameRepository : IBaseRepository<GameEntity>
    {
        List<GameEntity> GetFeaturedForHomePage();
        List<GameEntity> GetSpecialOffersForHomePage();
        GameEntity GetGameDetails(int id);
        GameEntity GetByTitle(string title);
        bool IsTitleFree(string title, int excludeGameId = 0);
        //PaginatedList<GameData> GetGames(GameFilter filter, int pageIndex, int pageSize);
        List<GameEntity> GetAllWithReviews();
    }
}
