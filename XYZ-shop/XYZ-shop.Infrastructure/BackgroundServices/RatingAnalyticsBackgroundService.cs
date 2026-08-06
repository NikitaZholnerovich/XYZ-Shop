using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XYZ_shop.Application.Abstractions.Services;

namespace XYZ_shop.Infrastructure.BackgroundServices
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
                using var scope = _serviceProvider.CreateScope();
                var ratingAnalyticsService = scope.ServiceProvider.GetRequiredService<IRatingAnalyticsService>();
                ratingAnalyticsService.RecalculateGameRatings();

                await Task.Delay(DelayBetweenRatingRecalculation, stoppingToken);
            }
        }
    }
}
