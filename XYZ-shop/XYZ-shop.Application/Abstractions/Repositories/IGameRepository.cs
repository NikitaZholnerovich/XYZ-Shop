
using XYZ_shop.Domain.Entities;
using XYZ_shop.Domain.HelperModels;
using XYZ_shop.Domain.HelperModels.Pagination;

namespace XYZ_shop.Application.Abstractions.Repositories
{
    public interface IGameRepository : IBaseRepository<GameEntity>
    {
        List<GameEntity> GetFeaturedForHomePage();
        List<GameEntity> GetSpecialOffersForHomePage();
        GameEntity GetGameDetails(int id);
        GameEntity GetByTitle(string title);
        bool IsTitleFree(string title, int excludeGameId = 0);
        PaginatedList<GameEntity> GetGames(GameFilter filter, int pageIndex, int pageSize);
        List<GameEntity> GetAllWithReviews();
    }
}
