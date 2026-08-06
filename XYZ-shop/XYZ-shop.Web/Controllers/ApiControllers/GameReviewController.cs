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

            var avatarUrl = _authService.GetUser()?.AvatarUrl;
            if (string.IsNullOrWhiteSpace(avatarUrl))
            {
                avatarUrl = "/images/default-avatar.png";
            }

            return Ok(new AddGameReviewApiResponse
            {
                IsSuccess = true,
                Id = review.Id,
                Author = _authService.GetUserName()!,
                AuthorAvatarUrl = avatarUrl,
                Text = review.Text,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            });
        }

        [HttpPost]
        public IActionResult Edit([FromBody] EditGameReviewRequest request)
        {
            if (!_authService.IsAuthenticated())
            {
                return Unauthorized(new ErrorApiResponse("Login required."));
            }

            if (request == null || request.Id <= 0 || request.Rating < 1 || request.Rating > 10)
            {
                return BadRequest(new ErrorApiResponse("Invalid data."));
            }

            var text = request.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text) || text.Length < 3 || text.Length > 5000)
            {
                return BadRequest(new ErrorApiResponse("Invalid data."));
            }

            var review = _gameReviewRepository.Get(request.Id);
            if (review == null)
            {
                return NotFound(new ErrorApiResponse("Review not found."));
            }

            if (!CanManage(review))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorApiResponse("Not allowed."));
            }

            review.Text = text;
            review.Rating = request.Rating;
            review.ModifiedAt = DateTime.UtcNow;
            _gameReviewRepository.Update(review);

            return Ok(new EditGameReviewApiResponse
            {
                IsSuccess = true,
                Id = review.Id,
                Text = review.Text,
                Rating = review.Rating,
                ModifiedAt = review.ModifiedAt.Value
            });
        }

        [HttpPost]
        public IActionResult Delete([FromBody] DeleteGameReviewRequest request)
        {
            if (!_authService.IsAuthenticated())
            {
                return Unauthorized(new ErrorApiResponse("Login required."));
            }

            if (request == null || request.Id <= 0)
            {
                return BadRequest(new ErrorApiResponse("Invalid data."));
            }

            var review = _gameReviewRepository.Get(request.Id);
            if (review == null)
            {
                return NotFound(new ErrorApiResponse("Review not found."));
            }

            if (!CanManage(review))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorApiResponse("Not allowed."));
            }

            _gameReviewRepository.Delete(request.Id);

            return Ok(new SuccessApiResponse());
        }

        private bool CanManage(GameReviewEntity review)
        {
            return review.AuthorId == _authService.GetUserId()
                || _authService.AtLeastModerator();
        }
    }
}
