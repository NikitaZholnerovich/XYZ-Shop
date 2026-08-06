using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Domain.Enums;
using XYZ_shop.Web.Auth;
using XYZ_shop.Web.Models.Profile;

namespace XYZ_shop.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly string _avatarsFolder = "images/avatars";
        private readonly string _defaultAvatarUrl = "/images/default-avatar.png";

        private readonly IAuthService _authService;
        private readonly IProfileService _profileService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(
            IAuthService authService,
            IProfileService profileService,
            IJwtTokenService jwtTokenService,
            IWebHostEnvironment environment)
        {
            _authService = authService;
            _profileService = profileService;
            _jwtTokenService = jwtTokenService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var profile = _profileService.GetProfile(_authService.GetUserId());
            if (profile == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(ToViewModel(profile));
        }

        [HttpPost]
        public IActionResult Index(ProfileViewModel viewModel)
        {
            var userId = _authService.GetUserId();
            var existing = _profileService.GetProfile(userId);
            if (existing == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                viewModel.Login = existing.Login;
                viewModel.AvatarUrl = ResolveAvatarUrl(existing.AvatarUrl);
                if (!Enum.IsDefined(typeof(Language), viewModel.Language))
                {
                    viewModel.Language = existing.Language;
                }

                return View(viewModel);
            }

            string? newAvatarUrl = null;
            if (viewModel.Avatar != null && viewModel.Avatar.Length > 0)
            {
                newAvatarUrl = SaveAvatar(userId, viewModel.Avatar, existing.AvatarUrl);
            }

            var result = _profileService.UpdateProfile(userId, new UpdateProfileDto
            {
                Email = viewModel.Email,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Mobilephone = viewModel.Mobilephone,
                BirthDate = viewModel.BirthDate,
                Language = viewModel.Language,
                NewAvatarUrl = newAvatarUrl,
            });

            if (!result.Success)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (result.LanguageChanged)
            {
                var user = _authService.GetUser();
                if (user != null)
                {
                    _jwtTokenService.WriteTokenToCookie(user);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private ProfileViewModel ToViewModel(ProfileDto profile)
        {
            return new ProfileViewModel
            {
                Login = profile.Login,
                Language = profile.Language,
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Mobilephone = profile.Mobilephone,
                BirthDate = profile.BirthDate,
                AvatarUrl = ResolveAvatarUrl(profile.AvatarUrl),
            };
        }

        private string ResolveAvatarUrl(string? avatarUrl)
        {
            return string.IsNullOrWhiteSpace(avatarUrl) ? _defaultAvatarUrl : avatarUrl;
        }

        private string SaveAvatar(int userId, IFormFile file, string? previousAvatarUrl)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var avatarsPath = Path.Combine(_environment.WebRootPath, "images", "avatars");
            Directory.CreateDirectory(avatarsPath);

            DeleteLocalAvatar(previousAvatarUrl);

            var fileName = $"{userId}{extension}";
            var fullPath = Path.Combine(avatarsPath, fileName);

            using (var stream = System.IO.File.Create(fullPath))
            {
                file.CopyTo(stream);
            }

            return $"/{_avatarsFolder}/{fileName}";
        }

        private void DeleteLocalAvatar(string? avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(avatarUrl))
            {
                return;
            }

            var prefix = $"/{_avatarsFolder}/";
            if (!avatarUrl.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var fileName = Path.GetFileName(avatarUrl);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            var fullPath = Path.Combine(_environment.WebRootPath, "images", "avatars", fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
