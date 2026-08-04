using Microsoft.AspNetCore.Mvc;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Web.CustomAuthAttributes.Api;

namespace XYZ_shop.Web.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly IAuthService _authService;

        public CatalogController(IGameRepository gameRepository, IAuthService authService)
        {
            _gameRepository = gameRepository;
            _authService = authService;
        }

        [IsAdminApi]
        public bool Delete([FromQuery] List<int> gameIds)
        {
            _gameRepository.Delete(gameIds);
            return true;
        }
    }
}
