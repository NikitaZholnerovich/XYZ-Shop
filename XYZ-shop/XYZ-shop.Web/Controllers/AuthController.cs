using Microsoft.AspNetCore.Mvc;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Web.Auth;
using XYZ_shop.Web.Models.Auth;

namespace XYZ_shop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IAuthService authService, IJwtTokenService jwtTokenService)
        {
            _authService = authService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = _authService.Login(viewModel.Login, viewModel.Password);
            if (user == null)
            {
                ModelState.AddModelError(
                    nameof(LoginViewModel.Login),
                    "There is no user with this login and password");
                return View(viewModel);
            }

            _jwtTokenService.WriteTokenToCookie(user);

            return RedirectToAction("Index", "Steam");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = _authService.Register(viewModel.Login, viewModel.Password);
            if (user == null)
            {
                ModelState.AddModelError(nameof(LoginViewModel.Login), "Name is already used");
                return View(viewModel);
            }

            _jwtTokenService.WriteTokenToCookie(user);
            return RedirectToAction("Index", "Steam");
        }

        public IActionResult Logout()
        {
            _jwtTokenService.ClearTokenCookie();
            return RedirectToAction("Index", "Steam");
        }

        public IActionResult Deny()
        {
            return View();
        }
    }
}
