using Microsoft.AspNetCore.Mvc;

using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Domain.Entities;
using XYZ_shop.Web.Models.Api;
using XYZ_shop.Web.Models.Api.GameReviewApi;

namespace XYZ_shop.Web.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameReviewController : ControllerBase
    {
        private readonly IGameReviewRepository _gameReviewRepository;
        private readonly IAuthService _authService;

        public GameReviewController(IGameReviewRepository reviews, IAuthService auth)
        {
            _gameReviewRepository = reviews;
            _authService = auth;
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddGameReviewRequest request)
        {
            if (!_authService.IsAuthenticated())
            {
                return Unauthorized(new ErrorApiResponse("Login required."));
            }

            if (request == null || request.GameId <= 0 || request.Rating < 1 || request.Rating > 10)
            {
                return BadRequest(new ErrorApiResponse("Invalid data."));
            }

            var text = request.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text) || text.Length < 3 || text.Length > 5000)
            {
                return BadRequest(new ErrorApiResponse("Invalid data."));
            }

            var userId = _authService.GetUserId();

            if (_gameReviewRepository.ExistsForUser(request.GameId, userId))
            {
                return Conflict(new ErrorApiResponse("You already reviewed this game."));
            }

            var review = new GameReviewEntity
            {
                GameId = request.GameId,
                AuthorId = userId,
                Text = text,
                Rating = request.Rating,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null
            };

            _gameReviewRepository.Add(review);

            return Ok(new AddGameReviewApiResponse
            {
                IsSuccess = true,
                Author = _authService.GetUserName()!,
                Text = review.Text,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            });
        }
    }
}
