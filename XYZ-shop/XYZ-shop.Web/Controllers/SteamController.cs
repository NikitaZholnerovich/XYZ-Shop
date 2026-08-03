using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Web.CustomAuthAttributes;
using XYZ_shop.Web.Mapping;
using XYZ_shop.Web.Models;

namespace XYZ_shop.Web.Controllers
{
    [Authorize]
    public class SteamController : Controller
    {
        private const int CatalogDefaultPageSize = 12;
        private const int CatalogMaxPageSize = 48;

        private readonly ICatalogService _catalogService;
        private readonly ICatalogViewModelMapper _catalogViewModelMapper;

        public SteamController(
            ICatalogService catalogService,
            ICatalogViewModelMapper catalogViewModelMapper)
        {
            _catalogService = catalogService;
            _catalogViewModelMapper = catalogViewModelMapper;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var catalog = _catalogService.GetGamesForHomePage();
            var viewModel = _catalogViewModelMapper.ToViewModel(catalog);

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Catalog([FromQuery] CatalogFilterViewModel filter)
        {
            filter ??= new CatalogFilterViewModel();
            if (filter.Page < 1)
            {
                filter.Page = 1;
            }

            if (filter.PageSize < 1)
            {
                filter.PageSize = CatalogDefaultPageSize;
            }
            else if (filter.PageSize > CatalogMaxPageSize)
            {
                filter.PageSize = CatalogMaxPageSize;
            }

            var catalog = _catalogService.GetCatalog(_catalogViewModelMapper.ToDto(filter));
            var model = _catalogViewModelMapper.ToViewModel(catalog);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GameDetails(int id)
        {
            var game = _catalogService.GetGameDetails(id);

            if (game == null)
            {
                return NotFound();
            }

            var viewModel = _catalogViewModelMapper.ToViewModel(game);

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Moderator,Admin")]
        public IActionResult AddGame()
        {
            var viewModel = _catalogViewModelMapper.ToViewModel(_catalogService.GetGameFormOptions());

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator,Admin")]
        public IActionResult AddGame(AddGameViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                _catalogViewModelMapper.FillOptions(viewModel, _catalogService.GetGameFormOptions());
                return View(viewModel);
            }

            _catalogService.AddGame(_catalogViewModelMapper.ToDto(viewModel));

            return RedirectToAction(nameof(Catalog));
        }

        [HttpGet]
        [EditForCreatorWithRequiredRole]
        [Authorize(Roles = "Moderator,Admin")]
        public IActionResult EditGame(int id)
        {
            var form = _catalogService.GetEditGameForm(id);

            if (form == null)
            {
                return NotFound();
            }

            var viewModel = _catalogViewModelMapper.ToViewModel(form);

            return View(viewModel);
        }

        [HttpPost]
        [EditForCreatorWithRequiredRole]
        [Authorize(Roles = "Moderator,Admin")]
        public IActionResult EditGame(EditGameViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                _catalogViewModelMapper.FillOptions(viewModel, _catalogService.GetGameFormOptions());
                return View(viewModel);
            }

            _catalogService.UpdateGame(_catalogViewModelMapper.ToDto(viewModel));

            return RedirectToAction(nameof(GameDetails), new { id = viewModel.Id });
        }
    }
}
