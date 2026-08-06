using Microsoft.AspNetCore.Mvc;
using XYZ_shop.Application.Abstractions.Services;
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

        [IsAdminApi]
        public bool Delete([FromQuery] List<int> gameIds)
        {
            _catalogService.DeleteGames(gameIds);
            return true;
        }
    }
}
