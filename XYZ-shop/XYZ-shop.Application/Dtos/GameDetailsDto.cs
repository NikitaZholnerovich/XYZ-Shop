namespace XYZ_shop.Application.Dtos
{
    public class GameDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double? AverageRating { get; set; }
        public int ReviewsCount { get; set; }
        public int PositiveReviewsCount { get; set; }
        public List<string> Genres { get; set; } = new();
        public string? PublisherName { get; set; }
        public int? PublisherId { get; set; }
        public List<GameReviewDto> Reviews { get; set; } = new();
    }
}
