using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using XYZ_shop.Application.Services;
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Web.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        public const string ACCESS_TOKEN_COOKIE = "access_token";

        private readonly JwtSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenService(IOptions<JwtSettings> settings, IHttpContextAccessor httpContextAccessor)
        {
            _settings = settings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(ClaimTypes.Name, user.Login),
                new(AuthService.LANGUAGE_CLAIM, user.Language.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpireMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void WriteTokenToCookie(UserEntity user)
        {
            var token = GenerateToken(user);
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext is not available");

            httpContext.Response.Cookies.Append(ACCESS_TOKEN_COOKIE, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = httpContext.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_settings.ExpireMinutes),
            });
        }

        public void ClearTokenCookie()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext is not available");

            httpContext.Response.Cookies.Delete(ACCESS_TOKEN_COOKIE);
        }
    }
}
