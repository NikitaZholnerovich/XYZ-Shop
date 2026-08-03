using XYZ_shop.Application.Dtos.Rawg;

namespace XYZ_shop.Web.Models
{
    public class RecommendationsViewModel
    {
        public string? SearchQuery { get; set; }
        public RawgGameDto? SelectedGame { get; set; }
        public List<RawgGameDto> GameSeries { get; set; } = new();
        public List<RawgGameDto> PopularGames { get; set; } = new();
        public List<RawgGameDto> NewReleases { get; set; } = new();
        public List<RawgGameDto> SearchResults { get; set; } = new();
        public bool HasSearchResults => SearchResults.Any();
        public bool HasPopularGames => PopularGames.Any();
        public bool HasGameSeries => GameSeries.Any();
    }
}
