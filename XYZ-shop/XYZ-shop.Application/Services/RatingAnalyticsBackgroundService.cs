
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XYZ_shop.Application.Abstractions.Repositories;

namespace XYZ_shop.Application.Services
{
    public class RatingAnalyticsBackgroundService : BackgroundService
    {
        public readonly TimeSpan DelayBetweenRatingRecalculation = TimeSpan.FromSeconds(30);

        private readonly IServiceProvider _serviceProvider;

        public RatingAnalyticsBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                RecalculateGameRatings();
                await Task.Delay(DelayBetweenRatingRecalculation, stoppingToken);
            }
        }

        private void RecalculateGameRatings()
        {
            using var scope = _serviceProvider.CreateScope();
            var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
            var games = gameRepository.GetAllWithReviews();

            foreach (var game in games)
            {
                var reviews = game.GameReviews ?? [];

                game.ReviewsCount = reviews.Count;
                game.PositiveReviewsCount = reviews.Count(review => review.Rating >= 7);
                game.AverageRating = reviews.Count > 0
                    ? Math.Round(reviews.Average(review => review.Rating), 1)
                    : null;
            }

            gameRepository.SaveChanges();
        }
    }
}
