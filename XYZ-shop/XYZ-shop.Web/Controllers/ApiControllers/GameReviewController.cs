using Microsoft.AspNetCore.Mvc;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Web.Models.Api;
using XYZ_shop.Web.Models.Api.GameReviewApi;

namespace XYZ_shop.Web.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameReviewController : ControllerBase
    {
        private readonly IGameReviewService _gameReviewService;

        public GameReviewController(IGameReviewService gameReviewService)
        {
            _gameReviewService = gameReviewService;
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddGameReviewRequest request)
        {
            var result = _gameReviewService.Add(new AddGameReviewDto
            {
                GameId = request?.GameId ?? 0,
                Text = request?.Text ?? string.Empty,
                Rating = request?.Rating ?? 0
            });

            return result.Status switch
            {
                GameReviewOperationStatus.Unauthorized => Unauthorized(new ErrorApiResponse("Login required.")),
                GameReviewOperationStatus.InvalidData => BadRequest(new ErrorApiResponse("Invalid data.")),
                GameReviewOperationStatus.AlreadyExists => Conflict(new ErrorApiResponse("You already reviewed this game.")),
                GameReviewOperationStatus.Success => Ok(new AddGameReviewApiResponse
                {
                    IsSuccess = true,
                    Id = result.Review!.Id,
                    Author = result.Review.AuthorName,
                    AuthorAvatarUrl = result.Review.AuthorAvatarUrl ?? "/images/default-avatar.png",
                    Text = result.Review.Text,
                    Rating = result.Review.Rating,
                    CreatedAt = result.Review.CreatedAt
                }),
                _ => BadRequest(new ErrorApiResponse("Invalid data."))
            };
        }

        [HttpPost]
        public IActionResult Edit([FromBody] EditGameReviewRequest request)
        {
            var result = _gameReviewService.Edit(new EditGameReviewDto
            {
                Id = request?.Id ?? 0,
                Text = request?.Text ?? string.Empty,
                Rating = request?.Rating ?? 0
            });

            return result.Status switch
            {
                GameReviewOperationStatus.Unauthorized => Unauthorized(new ErrorApiResponse("Login required.")),
                GameReviewOperationStatus.InvalidData => BadRequest(new ErrorApiResponse("Invalid data.")),
                GameReviewOperationStatus.NotFound => NotFound(new ErrorApiResponse("Review not found.")),
                GameReviewOperationStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden, new ErrorApiResponse("Not allowed.")),
                GameReviewOperationStatus.Success => Ok(new EditGameReviewApiResponse
                {
                    IsSuccess = true,
                    Id = result.Review!.Id,
                    Text = result.Review.Text,
                    Rating = result.Review.Rating,
                    ModifiedAt = result.Review.ModifiedAt!.Value
                }),
                _ => BadRequest(new ErrorApiResponse("Invalid data."))
            };
        }

        [HttpPost]
        public IActionResult Delete([FromBody] DeleteGameReviewRequest request)
        {
            var result = _gameReviewService.Delete(request?.Id ?? 0);

            return result.Status switch
            {
                GameReviewOperationStatus.Unauthorized => Unauthorized(new ErrorApiResponse("Login required.")),
                GameReviewOperationStatus.InvalidData => BadRequest(new ErrorApiResponse("Invalid data.")),
                GameReviewOperationStatus.NotFound => NotFound(new ErrorApiResponse("Review not found.")),
                GameReviewOperationStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden, new ErrorApiResponse("Not allowed.")),
                GameReviewOperationStatus.Success => Ok(new SuccessApiResponse()),
                _ => BadRequest(new ErrorApiResponse("Invalid data."))
            };
        }
    }
}
