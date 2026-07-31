
using XYZ_shop.Domain.Entities;
using XYZ_shop.Domain.Enums;

namespace XYZ_shop.Application.Abstractions.Services
{
    public interface IAuthService
    {
        UserEntity? Login(string login, string password);
        UserEntity Register(string login, string password);
        UserRole GetRole();
        UserEntity? GetUser();
        int GetUserId();
        string? GetUserName();
        bool IsAuthenticated();
        bool AtLeastModerator();
        bool IsUser();
        Language GetLanguage();
    }
}
