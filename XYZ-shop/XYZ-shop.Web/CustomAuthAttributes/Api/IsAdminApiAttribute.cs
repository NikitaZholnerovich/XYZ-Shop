using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Domain.Enums;

namespace XYZ_shop.Web.CustomAuthAttributes.Api
{
    public class IsAdminApiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();

            if (authService.GetRole() != UserRole.Admin)
            {
                context.Result = new ObjectResult(new
                {
                    error = "Access denied. Admin rights required."
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
