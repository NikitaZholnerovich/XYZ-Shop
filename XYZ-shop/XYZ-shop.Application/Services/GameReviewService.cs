using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;
using XYZ_shop.Application.Dtos;
using XYZ_shop.Domain.Entities;

namespace XYZ_shop.Application.Services
{
    public class GameReviewService : IGameReviewService
    {
        private const int MinRating = 1;
        private const int MaxRating = 10;
        private const int MinTextLength = 3;
        private const int MaxTextLength = 5000;
        private const string DefaultAvatarUrl = "/images/default-avatar.png";

        private readonly IGameReviewRepository _gameReviewRepository;
        private readonly IAuthService _authService;

        public GameReviewService(IGameReviewRepository gameReviewRepository, IAuthService authService)
        {
            _gameReviewRepository = gameReviewRepository;
            _authService = authService;
        }

        public AddGameReviewResultDto Add(AddGameReviewDto dto)
        {
            if (!_authService.IsAuthenticated())
            {
                return new AddGameReviewResultDto { Status = GameReviewOperationStatus.Unauthorized };
            }

            if (dto == null || dto.GameId <= 0 || dto.Rating < MinRating || dto.Rating > MaxRating)
            {
                return new AddGameReviewResultDto { Status = GameReviewOperationStatus.InvalidData };
            }

            var text = dto.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text) || text.Length < MinTextLength || text.Length > MaxTextLength)
            {
                return new AddGameReviewResultDto { Status = GameReviewOperationStatus.InvalidData };
            }

            var userId = _authService.GetUserId();

            if (_gameReviewRepository.ExistsForUser(dto.GameId, userId))
            {
                return new AddGameReviewResultDto { Status = GameReviewOperationStatus.AlreadyExists };
            }

            var review = new GameReviewEntity
            {
                GameId = dto.GameId,
                AuthorId = userId,
                Text = text,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null
            };

            _gameReviewRepository.Add(review);

            return new AddGameReviewResultDto
            {
                Status = GameReviewOperationStatus.Success,
                Review = new GameReviewDto
                {
                    Id = review.Id,
                    GameId = review.GameId,
                    AuthorId = review.AuthorId,
                    AuthorName = _authService.GetUserName()!,
                    AuthorAvatarUrl = ResolveAvatarUrl(_authService.GetUser()?.AvatarUrl),
                    Text = review.Text,
                    Rating = review.Rating,
                    IsRecommended = review.Rating >= 7,
                    CreatedAt = review.CreatedAt,
                    ModifiedAt = review.ModifiedAt
                }
            };
        }

        public EditGameReviewResultDto Edit(EditGameReviewDto dto)
        {
            if (!_authService.IsAuthenticated())
            {
                return new EditGameReviewResultDto { Status = GameReviewOperationStatus.Unauthorized };
            }

            if (dto == null || dto.Id <= 0 || dto.Rating < MinRating || dto.Rating > MaxRating)
            {
                return new EditGameReviewResultDto { Status = GameReviewOperationStatus.InvalidData };
            }

            var text = dto.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text) || text.Length < MinTextLength || text.Length > MaxTextLength)
            {
                return new EditGameReviewResultDto { Status = GameReviewOperationStatus.InvalidData };
            }

            var review = _gameReviewRepository.Get(dto.Id);
            if (review == null)
            {
                return new EditGameReviewResultDto { Status = GameReviewOperationStatus.NotFound };
            }

            if (!CanManage(review))
            {
                return new EditGameReviewResultDto { Status = GameReviewOperationStatus.Forbidden };
            }

            review.Text = text;
            review.Rating = dto.Rating;
            review.ModifiedAt = DateTime.UtcNow;
            _gameReviewRepository.Update(review);

            return new EditGameReviewResultDto
            {
                Status = GameReviewOperationStatus.Success,
                Review = new GameReviewDto
                {
                    Id = review.Id,
                    GameId = review.GameId,
                    AuthorId = review.AuthorId,
                    Text = review.Text,
                    Rating = review.Rating,
                    IsRecommended = review.Rating >= 7,
                    CreatedAt = review.CreatedAt,
                    ModifiedAt = review.ModifiedAt
                }
            };
        }

        public DeleteGameReviewResultDto Delete(int reviewId)
        {
            if (!_authService.IsAuthenticated())
            {
                return new DeleteGameReviewResultDto { Status = GameReviewOperationStatus.Unauthorized };
            }

            if (reviewId <= 0)
            {
                return new DeleteGameReviewResultDto { Status = GameReviewOperationStatus.InvalidData };
            }

            var review = _gameReviewRepository.Get(reviewId);
            if (review == null)
            {
                return new DeleteGameReviewResultDto { Status = GameReviewOperationStatus.NotFound };
            }

            if (!CanManage(review))
            {
                return new DeleteGameReviewResultDto { Status = GameReviewOperationStatus.Forbidden };
            }

            _gameReviewRepository.Delete(reviewId);

            return new DeleteGameReviewResultDto { Status = GameReviewOperationStatus.Success };
        }

        private bool CanManage(GameReviewEntity review)
        {
            return review.AuthorId == _authService.GetUserId()
                || _authService.AtLeastModerator();
        }

        private static string ResolveAvatarUrl(string? avatarUrl)
        {
            return string.IsNullOrWhiteSpace(avatarUrl) ? DefaultAvatarUrl : avatarUrl;
        }
    }
}
