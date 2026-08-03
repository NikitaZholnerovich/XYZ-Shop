using Microsoft.AspNetCore.Mvc;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos.Rawg;
using XYZ_shop.Web.Models;

namespace XYZ_shop.Web.Controllers
{
    [Route("Steam/Recommendations")]
    public class RecommendationsController : Controller
    {
        private readonly IRawgApi _rawgApi;
        private const int BASE_GAME_NUMBER_FOR_RECOMMENDATIONS = 12;
        private const int BASE_GAME_SERIES_NUMBER_FOR_RECOMMENDATIONS = 12;
        private const int BASE_GAME_SERIES_NUMBER_FOR_DETAILS_PAGE = 6;

        public RecommendationsController(IRawgApi rawgApi)
        {
            _rawgApi = rawgApi;
        }

        [HttpGet]
        public async Task<IActionResult> IndexRecommendations(string? searchQuery = null)
        {
            var viewModel = new RecommendationsViewModel { SearchQuery = searchQuery };

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var searchResults = await _rawgApi.SearchGames(searchQuery, 10);
                viewModel.SearchResults = searchResults?.Results ?? new List<RawgGameDto>();

                if (viewModel.SearchResults.Count == 1)
                {
                    var game = viewModel.SearchResults[0];
                    var series = await _rawgApi.GetGameSeries(game.Id.ToString(), BASE_GAME_SERIES_NUMBER_FOR_RECOMMENDATIONS);
                    viewModel.SelectedGame = game;
                    viewModel.GameSeries = series?.Results ?? new List<RawgGameDto>();
                }
            }
            else
            {
                var popular = await _rawgApi.GetPopularGames(BASE_GAME_NUMBER_FOR_RECOMMENDATIONS);
                viewModel.PopularGames = popular?.Results ?? new List<RawgGameDto>();

                var newReleases = await _rawgApi.GetNewReleases(BASE_GAME_NUMBER_FOR_RECOMMENDATIONS);
                viewModel.NewReleases = newReleases?.Results ?? new List<RawgGameDto>();
            }
            return View(viewModel);
        }

        [HttpGet("Game/{slug}")]
        public async Task<IActionResult> GameDetails(string slug)
        {
            var game = await _rawgApi.GetGameDetails(slug);
            if (game == null)
            {
                return NotFound();
            }

            var series = await _rawgApi.GetGameSeries(game.Id.ToString(), BASE_GAME_SERIES_NUMBER_FOR_DETAILS_PAGE);

            var viewModel = new RecommendationsViewModel
            {
                SelectedGame = game,
                GameSeries = series?.Results ?? new List<RawgGameDto>()
            };
            return View(viewModel);
        }
    }
}
