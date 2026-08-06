namespace XYZ_shop.Web.Models
{
    public class GameDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsUserAtLeastModerator { get; set; }
        public double? AverageRating { get; set; }
        public int ReviewsCount { get; set; }
        public int PositiveReviewsCount { get; set; }

        public List<GenreLinkViewModel> Genres { get; set; } = new();
        public string? PublisherName { get; set; }
        public int? PublisherId { get; set; }
        public List<GameReviewViewModel> Reviews { get; set; } = new();
        public List<SteamGameViewModel> SimilarGames { get; set; } = new();
        public bool HasUserReviewed { get; set; }
    }
}
