using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Domain.Enums;

namespace XYZ_shop.Application.Services
{
    public class AuthService : IAuthService
    {
        public const string LANGUAGE_CLAIM = "language";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public UserEntity? Login(string login, string password)
        {
            var user = _userRepository.GetByLogin(login);
            if (user == null || !_passwordHasher.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public UserEntity? Register(string login, string password)
        {
            if (!_userRepository.IsLoginUnique(login))
            {
                return null;
            }

            var user = new UserEntity
            {
                Login = login,
                PasswordHash = _passwordHasher.Hash(password),
            };

            _userRepository.Register(user);
            return user;
        }

        public int GetUserId()
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User
                .Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            if (userIdStr is null)
            {
                return 0;
            }

            return int.Parse(userIdStr);
        }

        public UserEntity? GetUser()
        {
            var userId = GetUserId();
            if (userId <= 0)
            {
                return null;
            }

            return _userRepository.Get(userId);
        }

        public string? GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User
                .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)
                ?.Value;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        public UserRole GetRole()
        {
            if (!IsAuthenticated())
            {
                throw new InvalidOperationException("User is not authenticated");
            }

            var roleStr = _httpContextAccessor.HttpContext!.User
                .Claims.First(x => x.Type == ClaimTypes.Role)
                .Value;
            return Enum.Parse<UserRole>(roleStr!);
        }

        public bool AtLeastModerator()
        {
            if (!IsAuthenticated())
            {
                return false;
            }

            var role = GetRole();
            return role == UserRole.Moderator || role == UserRole.Admin;
        }

        public Language GetLanguage()
        {
            if (!IsAuthenticated())
            {
                return Language.English;
            }

            var languageStr = _httpContextAccessor.HttpContext!.User
                .Claims.First(x => x.Type == LANGUAGE_CLAIM)
                .Value;
            return Enum.Parse<Language>(languageStr!);
        }

        public bool IsUser()
        {
            return IsAuthenticated() && GetRole() == UserRole.User;
        }
    }
}
