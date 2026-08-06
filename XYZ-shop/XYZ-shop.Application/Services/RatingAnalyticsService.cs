using XYZ_shop.Application.Abstractions.Repositories;
using XYZ_shop.Application.Abstractions.Services;

namespace XYZ_shop.Application.Services
{
    public class RatingAnalyticsService : IRatingAnalyticsService
    {
        private const int PositiveReviewRatingThreshold = 7;

        private readonly IGameRepository _gameRepository;

        public RatingAnalyticsService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public void RecalculateGameRatings()
        {
            var games = _gameRepository.GetAllWithReviews();

            foreach (var game in games)
            {
                var reviews = game.GameReviews ?? [];

                game.ReviewsCount = reviews.Count;
                game.PositiveReviewsCount = reviews.Count(review => review.Rating >= PositiveReviewRatingThreshold);
                game.AverageRating = reviews.Count > 0
                    ? Math.Round(reviews.Average(review => review.Rating), 1)
                    : null;
            }

            _gameRepository.SaveChanges();
        }
    }
}
