using System.Globalization;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Domain.Enums;

namespace XYZ_shop.Web.Middleware
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            var culture = new CultureInfo("en-US");

            if (authService.IsAuthenticated() && authService.GetLanguage() == Language.Russian)
            {
                culture = new CultureInfo("ru-RU");
            }

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            await _next(context);
        }
    }
}
