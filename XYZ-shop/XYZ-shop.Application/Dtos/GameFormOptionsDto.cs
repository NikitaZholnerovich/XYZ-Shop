namespace XYZ_shop.Application.Dtos
{
    public class GameFormOptionsDto
    {
        public List<CatalogGenreDto> Genres { get; set; } = new();
        public List<PublisherDto> Publishers { get; set; } = new();
    }
}
