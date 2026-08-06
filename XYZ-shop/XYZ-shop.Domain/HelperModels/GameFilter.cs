
namespace XYZ_shop.Domain.HelperModels
{
    public class GameFilter
    {
        public int? GenreId { get; set; }
        public int? PublisherId { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
