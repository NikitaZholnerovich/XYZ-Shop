namespace XYZ_shop.Application.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double? AverageRating { get; set; }
        public int ReviewsCount { get; set; }
        public string? Description { get; set; }
        public List<string> Genres { get; set; } = new();
    }
}
