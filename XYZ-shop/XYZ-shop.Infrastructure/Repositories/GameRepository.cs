using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Domain.HelperModels;
using XYZ_shop.Domain.HelperModels.Pagination;
using XYZ_shop.Infrastructure.Data;

namespace XYZ_shop.Infrastructure.Repositories
{
    public class GameRepository : BaseRepository<GameEntity>, IGameRepository
    {
        public const int SPECIAL_OFFERS_PREVIEW_COUNT = 15;

        public GameRepository(XyzDbContext context) : base(context)
        {
        }

        public List<GameEntity> GetFeaturedForHomePage()
        {
            var featured = _dbSet
                .Include(g => g.GameGenres)
                .Skip(SPECIAL_OFFERS_PREVIEW_COUNT).ToList();

            return featured;
        }

        public List<GameEntity> GetSpecialOffersForHomePage()
        {
            var specialOffers = _dbSet
                .Include(g => g.GameGenres)
                .Take(SPECIAL_OFFERS_PREVIEW_COUNT).ToList();

            return specialOffers;
        }

        public GameEntity GetGameDetails(int id)
        {
            var gameData = _dbSet
                .Include(g => g.Publisher)
                .Include(g => g.GameGenres)
                .Include(g => g.GameReviews)
                    .ThenInclude(r => r.Author)
                .FirstOrDefault(g => g.Id == id);
            return gameData;
        }

        public GameEntity GetByTitle(string title)
        {
            return _dbSet.FirstOrDefault(g => g.Title == title);
        }

        public bool IsTitleFree(string title, int excludeGameId = 0)
        {
            return !_dbSet.Any(x => x.Title == title && x.Id != excludeGameId);
        }

        public List<GameEntity> GetAllWithReviews()
        {
            return _dbSet
                .Include(g => g.GameReviews)
                .ToList();
        }

        public PaginatedList<GameEntity> GetGames(GameFilter filter, int pageIndex, int pageSize)
        {
            var games = _dbSet
               .Include(g => g.GameGenres)
               .AsQueryable();

            if (filter.GenreId.HasValue)
            {
                games = games.Where(g => g.GameGenres.Any(gg => gg.Id == filter.GenreId.Value));
            }

            if (filter.PublisherId.HasValue)
            {
                games = games.Where(g => g.PublisherId == filter.PublisherId.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                games = games.Where(g => g.Price <= filter.MaxPrice.Value);
            }

            var count = games.Count();
            var totalPages = count == 0 ? 1 : (int)Math.Ceiling(count / (double)pageSize);
            var safePageIndex = Math.Min(Math.Max(1, pageIndex), totalPages);

            var sortedGames = ApplySorting(games, filter.SortBy, filter.SortDirection);

            var pageItems = sortedGames
                .Skip((safePageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedList<GameEntity>(pageItems, safePageIndex, totalPages, count);
        }

        public IQueryable<T> ApplySorting<T>(
            IQueryable<T> query,
            string? sortBy,
            string? sortDirection)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return query;
            }

            var propertyInfo = typeof(T).GetProperty(sortBy,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                return query;
            }

            var parameter = Expression.Parameter(typeof(T), "entity");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase)
                ? nameof(Queryable.OrderByDescending)
                : nameof(Queryable.OrderBy);

            var orderByMethod = typeof(Queryable)
                .GetMethods()
                .First(method => method.Name == methodName
                    && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), propertyInfo.PropertyType);

            return (IQueryable<T>)orderByMethod.Invoke(null, [query, lambda])!;
        }
    }
}
