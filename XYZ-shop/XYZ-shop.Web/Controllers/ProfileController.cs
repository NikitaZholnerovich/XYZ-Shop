using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Domain.Entities;
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
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(
            IAuthService authService,
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IWebHostEnvironment environment)
        {
            _authService = authService;
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var user = _userRepository.GetWithProfile(_authService.GetUserId());
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(ToViewModel(user));
        }

        [HttpPost]
        public IActionResult Index(ProfileViewModel viewModel)
        {
            var userId = _authService.GetUserId();
            var user = _userRepository.GetWithProfile(userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                viewModel.Login = user.Login;
                viewModel.AvatarUrl = ResolveAvatarUrl(user.AvatarUrl);
                if (!Enum.IsDefined(typeof(Language), viewModel.Language))
                {
                    viewModel.Language = user.Language;
                }

                return View(viewModel);
            }

            if (!Enum.IsDefined(typeof(Language), viewModel.Language))
            {
                viewModel.Language = user.Language;
            }

            string? newAvatarUrl = null;
            if (viewModel.Avatar != null && viewModel.Avatar.Length > 0)
            {
                newAvatarUrl = SaveAvatar(userId, viewModel.Avatar, user.AvatarUrl);
            }

            _userRepository.UpdateProfile(new UserEntity
            {
                Id = userId,
                AvatarUrl = newAvatarUrl,
                UserProfile = new UserProfileEntity
                {
                    Email = viewModel.Email.Trim(),
                    FirstName = viewModel.FirstName?.Trim(),
                    LastName = viewModel.LastName?.Trim(),
                    Mobilephone = viewModel.Mobilephone?.Trim(),
                    BirthDate = viewModel.BirthDate,
                }
            });

            if (viewModel.Language is Language.English or Language.Russian
                && viewModel.Language != user.Language)
            {
                _userRepository.UpdateLanguage(userId, viewModel.Language);
                user.Language = viewModel.Language;
                _jwtTokenService.WriteTokenToCookie(user);
            }

            return RedirectToAction(nameof(Index));
        }

        private ProfileViewModel ToViewModel(UserEntity user)
        {
            return new ProfileViewModel
            {
                Login = user.Login,
                Language = user.Language,
                Email = user.UserProfile?.Email ?? string.Empty,
                FirstName = user.UserProfile?.FirstName,
                LastName = user.UserProfile?.LastName,
                Mobilephone = user.UserProfile?.Mobilephone,
                BirthDate = user.UserProfile?.BirthDate,
                AvatarUrl = ResolveAvatarUrl(user.AvatarUrl),
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
