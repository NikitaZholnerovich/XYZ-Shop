namespace XYZ_shop.Web.Models
{
    public class CatalogFilterViewModel
    {
        public int? GenreId { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
