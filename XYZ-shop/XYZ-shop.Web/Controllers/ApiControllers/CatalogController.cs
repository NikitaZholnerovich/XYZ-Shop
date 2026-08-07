using Microsoft.AspNetCore.Mvc;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Web.CustomAuthAttributes.Api;

namespace XYZ_shop.Web.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public ActionResult<PaginatedResponseDto<GameDto>> GetGames([FromQuery] CatalogFilterDto filter)
        {
            var catalog = _catalogService.GetCatalog(filter);
            var meta = catalog.PaginationMetadata;

            return Ok(new PaginatedResponseDto<GameDto>
            {
                Items = catalog.Games,
                TotalCount = meta.TotalCount,
                PageSize = meta.PageSize,
                CurrentPage = meta.CurrentPage,
                TotalPages = meta.TotalPages,
                HasPrevious = meta.HasPreviousPage,
                HasNext = meta.HasNextPage,
            });
        }

        [HttpGet]
        public ActionResult<GameDto> GetGameDetails([FromQuery] int id)
        {
            var game = _catalogService.GetGameDetails(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(new GameDto
            {
                Id = game.Id,
                Title = game.Title,
                ImageUrl = game.ImageUrl,
                Price = game.Price,
                AverageRating = game.AverageRating,
                ReviewsCount = game.ReviewsCount,
                Description = game.Description,
                Genres = game.Genres.Select(g => g.Name).ToList(),
            });
        }

        [IsAdminApi]
        public bool Delete([FromQuery] List<int> gameIds)
        {
            _catalogService.DeleteGames(gameIds);
            return true;
        }
    }
}
