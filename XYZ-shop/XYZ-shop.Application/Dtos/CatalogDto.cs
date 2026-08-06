namespace XYZ_shop.Application.Dtos
{
    public class CatalogDto
    {
        public CatalogFilterDto Filter { get; set; } = new();
        public List<GameDto> Games { get; set; } = new();
        public List<CatalogGenreDto> GameGenres { get; set; } = new();
        public List<PublisherDto> Publishers { get; set; } = new();
        public PaginationMetadataDto PaginationMetadata { get; set; } = new();
    }
}
