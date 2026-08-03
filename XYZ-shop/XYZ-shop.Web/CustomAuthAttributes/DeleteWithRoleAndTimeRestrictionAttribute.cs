using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Domain.Enums;

namespace XYZ_shop.Web.CustomAuthAttributes
{
    /// <summary>
    /// Allows game deletion only for:
    /// <list type="bullet">
    ///   <item><description>Admin users (always)</description></item>
    ///   <item><description>Game owners who have the required role within the allowed time limit (default: 3 days, Moderator role)</description></item>
    /// </list>
    /// </summary>
    public class DeleteWithRoleAndTimeRestrictionAttribute : ActionFilterAttribute
    {
        public int AllowedDaysForCreator { get; set; } = 3;
        public UserRole RequiredRoleForCreator { get; set; } = UserRole.Moderator;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!int.TryParse(context.RouteData.Values["id"]?.ToString(), out int gameId))
            {
                context.Result = new BadRequestResult();
                return;
            }

            var catalogService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<ICatalogService>();

            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<IAuthService>();

            var gameCreatedAt = catalogService.GetGameCreatedAt(gameId);
            if (gameCreatedAt == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            var currentUserId = authService.GetUserId();
            if (currentUserId == 0)
            {
                context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            var userRole = authService.GetRole();
            var isAdmin = userRole == UserRole.Admin;
            var hasRequiredRole = userRole == RequiredRoleForCreator;
            var isOwner = catalogService.IsUserCreatorOfTheGame(currentUserId, gameId);

            var daysSinceCreation = (DateTime.UtcNow - gameCreatedAt.Value).TotalDays;
            var isWithinTimeLimit = daysSinceCreation <= AllowedDaysForCreator;

            var canDelete = isAdmin || (isWithinTimeLimit && isOwner && hasRequiredRole);

            if (!canDelete)
            {
                context.Result = ((Controller)context.Controller)
                    .RedirectToAction("Deny", "Auth");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
