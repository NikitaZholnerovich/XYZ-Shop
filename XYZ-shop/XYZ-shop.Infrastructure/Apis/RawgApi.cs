using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos.Rawg;

namespace XYZ_shop.Infrastructure.Apis
{
    public class RawgApi : IRawgApi
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RawgApi(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["RAWG:ApiKey"]
                ?? throw new InvalidOperationException("RAWG:ApiKey is not configured");
        }

        public async Task<RawgResponse?> SearchGames(string query, int pageSize = 10) =>
            await _httpClient.GetFromJsonAsync<RawgResponse>(
                $"games?key={_apiKey}&search={Uri.EscapeDataString(query)}&page_size={pageSize}",
                JsonOptions);

        public async Task<RawgGameDto?> GetGameDetails(string slug) =>
            await _httpClient.GetFromJsonAsync<RawgGameDto>(
                $"games/{slug}?key={_apiKey}",
                JsonOptions);

        public async Task<RawgResponse?> GetGameSeries(string gameId, int pageSize = 6) =>
            await _httpClient.GetFromJsonAsync<RawgResponse>(
                $"games/{gameId}/game-series?key={_apiKey}&page_size={pageSize}",
                JsonOptions);

        public async Task<RawgResponse?> GetPopularGames(int pageSize = 12) =>
            await _httpClient.GetFromJsonAsync<RawgResponse>(
                $"games?key={_apiKey}&ordering=-rating&page_size={pageSize}",
                JsonOptions);

        public async Task<RawgResponse?> GetNewReleases(int pageSize = 12) =>
            await _httpClient.GetFromJsonAsync<RawgResponse>(
                $"games?key={_apiKey}&ordering=-released&page_size={pageSize}",
                JsonOptions);
    }
}
