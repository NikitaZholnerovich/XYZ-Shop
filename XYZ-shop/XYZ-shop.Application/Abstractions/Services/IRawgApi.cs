using XYZ_shop.Application.Dtos.Rawg;

namespace XYZ_shop.Application.Abstractions.Services
{
    public interface IRawgApi
    {
        Task<RawgResponse?> SearchGames(string query, int pageSize = 10);
        Task<RawgGameDto?> GetGameDetails(string slug);
        Task<RawgResponse?> GetGameSeries(string gameId, int pageSize = 6);
        Task<RawgResponse?> GetPopularGames(int pageSize = 12);
        Task<RawgResponse?> GetNewReleases(int pageSize = 12);
    }
}
