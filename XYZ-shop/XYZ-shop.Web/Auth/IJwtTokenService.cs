using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Web.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserEntity user);
        void WriteTokenToCookie(UserEntity user);
        void ClearTokenCookie();
    }
}
