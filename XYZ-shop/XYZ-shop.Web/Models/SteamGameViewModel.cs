namespace XYZ_shop.Web.Models
{
    public class SteamGameViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double? AverageRating { get; set; }
        public int ReviewsCount { get; set; }
        public List<string> Genres { get; set; } = new();
        public string? Description { get; set; }
    }
}
