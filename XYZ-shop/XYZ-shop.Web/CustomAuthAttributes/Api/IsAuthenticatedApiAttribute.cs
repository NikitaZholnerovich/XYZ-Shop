using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XYZ_shop.Application.Abstractions.Services;

namespace XYZ_shop.Web.CustomAuthAttributes.Api
{
    public class IsAuthenticatedApiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();
            if (!authService.IsAuthenticated())
            {
                context.Result = new ObjectResult(new
                {
                    error = "Authentication required."
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
